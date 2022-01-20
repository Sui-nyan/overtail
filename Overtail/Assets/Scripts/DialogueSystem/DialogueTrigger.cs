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

        public void SetDialogueObject(DialogueObject dialogueObject)
        {
            // maybe more logic
            this.dialogueObject = dialogueObject;
        }
        
        public void Interact(Interactor interactor)
        {
            if (DialogueManager.Instance.IsOpen) return;

            foreach (DialogueResposeEvents resposeEvents in GetComponents<DialogueResposeEvents>())
            {
                if (resposeEvents.DialogueObject == dialogueObject)
                {
                    DialogueManager.Instance.AddResponseEvents(resposeEvents.Events);
                    break;
                }
            }

            DialogueManager.Instance.StartDialogue(dialogueObject, GetComponent<NPC>());
        }
    }
}