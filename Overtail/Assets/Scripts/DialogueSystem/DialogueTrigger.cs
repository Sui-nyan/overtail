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
            if (collision.CompareTag("Player") && collision.TryGetComponent(out PlayerMovement player))
            {
                player.interactable = this;
                Debug.Log("Trigger me Elmo�");
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && collision.TryGetComponent(out PlayerMovement player))
            {
                if (player.interactable is DialogueTrigger dialogueActivatior && dialogueActivatior == this)
                {
                    player.interactable = null;
                }
            }
        }

        public void Interact(PlayerMove player)
        {
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

        public void Interact(PlayerMovement player)
        {
            Debug.Log("Trying to interact");
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
