using Overtail.PlayerModule;
using System.Collections;
using System.Collections.Generic;
using Overtail.NPCs;
using UnityEngine;

namespace Overtail.Dialogue
{
    public class DialogueTrigger : MonoBehaviour, IInteractable
    {

        [SerializeField] DialogueObject dialogueObject;

        public void UpdateDialogueObject(DialogueObject dialogueObject)
        {
            this.dialogueObject = dialogueObject;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out PlayerTalking player))
            {
                player.interactable = this;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out PlayerTalking player))
            {
                if (player.interactable is DialogueTrigger dialogueActivator && dialogueActivator == this)
                {
                    player.interactable = null;
                }
            }
        }

        public void Interact(IInteractor interactor)
        {
            if (interactor.GetType() != typeof(PlayerTalking)) return;
            PlayerTalking player = (PlayerTalking) interactor;

            foreach (DialogueResposeEvents resposeEvents in GetComponents<DialogueResposeEvents>())
            {
                if (resposeEvents.DialogueObject == dialogueObject)
                {
                    player.DialogueManager.AddResponseEvents(resposeEvents.Events);
                    break;
                }
            }
            player.DialogueManager.StartDialogue(dialogueObject, GetComponent<NPC>());
        }
    }
}
