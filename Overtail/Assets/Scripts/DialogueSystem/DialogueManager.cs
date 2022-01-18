using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Overtail.NPCs;
using System.Linq;
using System.Text.RegularExpressions;

namespace Overtail.Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private GameObject dialogueBox;

        public bool IsOpen { get; private set; }
        public TMP_Text nameText;
        public Image NPCSprite;
        public Sprite protraitObj;
        public TMP_Text dialogueText;
        private NPC _currentNPC;

        private ResponseHandler responseHandler;
        private TextWriter textWriter;
        private char[] seperator = { '{', '}' };

        private void Start()
        {
            textWriter = GetComponent<TextWriter>();
            responseHandler = GetComponent<ResponseHandler>();

            CloseDialogue();
        }

        public void StartDialogue(DialogueObject dialogueObject, NPC npc = null)
        {
            //if(dialogueObject.npc != null)
            //{
            //    nameText.text = dialogueObject.npc.NPCName;
            //    NPCSprite.sprite = dialogueObject.npc.portrait.sprite;
            //}

            if (npc != null) _currentNPC = npc;
         
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

                Regex regex = new Regex(@"{[A-Za-z]*}");
                string emotionSpecifier = regex.Match(dialogue).Value;

                if (!emotionSpecifier.Equals("")) // if emotion tag found
                {
                    dialogue = dialogue.Replace(emotionSpecifier, "").Trim();
                }
                string emotion = emotionSpecifier.Replace("{", "").Replace("}", "").Trim();
                TrySetSprite(emotion);

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

        private void TrySetSprite(string emotion)
        {
            Debug.LogWarning(_currentNPC.Name + "::" + emotion);
            NPCSprite.sprite = emotion switch
            {
                "angry" => _currentNPC.portrait.Angry,
                "happy" => _currentNPC.portrait.Happy,
                "special" => _currentNPC.portrait.Special,
                _ => _currentNPC.portrait.Neutral
            };
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

