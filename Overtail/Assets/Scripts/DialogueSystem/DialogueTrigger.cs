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
            Debug.Log("Hello");
            if (collision.TryGetComponent(out PlayerTalking player))
            {
                
                player.interactable = this;
                Debug.Log("Trigger me Elmo²");
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out PlayerTalking player))
            {
                if (player.interactable is DialogueTrigger dialogueActivatior && dialogueActivatior == this)
                {
                    player.interactable = null;
                }
            }
        }

        public void Interact(PlayerTalking player)
        {
            Debug.Log("Interact");
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
