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

        private ResponseHandler responseHandler;
        private TextWriter textWriter;
        private char[] seperator = { '{', '}' };

        private void Start()
        {
            textWriter = GetComponent<TextWriter>();
            responseHandler = GetComponent<ResponseHandler>();

            CloseDialogue();
        }

        public void StartDialogue(DialogueObject dialogueObject)
        {
            //if(dialogueObject.npc != null)
            //{
            //    nameText.text = dialogueObject.npc.NPCName;
            //    NPCSprite.sprite = dialogueObject.npc.portrait.sprite;
            //}
         
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
                string[] temp = SeperateText(dialogueObject.Dialogue[i]);
                string dialogue;

                if(temp.Length < 1)
                {
                    string emotion_id = temp[0];
                    //dialogueObject.npc.SetPortairt("emotion");
                    dialogue = temp[1];
                }
                else
                {
                    dialogue = string.Join("", temp);
                }

                
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

        string[] SeperateText(string text)
        {
            string[] temp = text.Split(seperator);
            return temp;
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

