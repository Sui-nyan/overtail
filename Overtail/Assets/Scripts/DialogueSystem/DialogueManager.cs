using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Overtail.NPC;
using System.Linq;

namespace Overtail.Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private GameObject dialogueBox;

        public bool IsOpen { get; private set; }
        public TMP_Text nameText;
        public Image NPCSprite;
        public TMP_Text dialogueText;
        public NPCObject NPC;

        private ResponseHandler responseHandler;
        private TextWriter textWriter;

        private void Start()
        {
            textWriter = GetComponent<TextWriter>();
            responseHandler = GetComponent<ResponseHandler>();

            CloseDialogue();
        }

        public void StartDialogue(DialogueObject dialogueObject)
        {
            nameText.text = dialogueObject.npc.name;
            IsOpen = true;
            dialogueBox.SetActive(true);
            StartCoroutine(StepThroughDialogue(dialogueObject));
        }

        public void AddResponseEvents(ResponseEvent[] responseEvents)
        {
            responseHandler.AddResponseEvents(responseEvents);
        }

        /*
         * Iterates through each line of the dialogue new line will appear as soon as the player clicks any key
         */
        private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
        {

            for (int i = 0; i < dialogueObject.Dialogue.Length; i++)
            {
                string dialogue = dialogueObject.Dialogue[i];
                yield return textWriter.Run(dialogue, dialogueText);

                if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses)
                {
                    break;
                }

                yield return new WaitUntil(() => Input.anyKeyDown);
            }

            if (dialogueObject.HasResponses)
            {
                responseHandler.showResponses(dialogueObject.Responses);
            }
            else
                CloseDialogue();
        }

        public void UpdatePortrait(string emotion)
        {
            /*Sprite sprite = NPC.emotions.FirstOrDefault(x => x.ID == emotion);
            if(sprite != null)
            {
                NPCSprite.sprite = sprite;
            }*/
        }

        public void CloseDialogue()
        {
            dialogueBox.SetActive(false);
            dialogueText.text = string.Empty;
            nameText.text = string.Empty;
            IsOpen = false;
        }
    }
}

