using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Overtail.NPCs;
using System.Linq;
using System.Text.RegularExpressions;
using Overtail.Util;

namespace Overtail.Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private GameObject dialogueBox;
        
        private static DialogueManager _instance;
        public static DialogueManager Instance => _instance;
        public bool IsOpen { get; private set; }
        public TMP_Text nameText;
        public Image NPCSprite;
        public TMP_Text dialogueText;

        private NPC _currentNPC;

        private ResponseHandler responseHandler;
        private TextWriter textWriter;

        private void Awake()
        {
            MonoBehaviourExtension.MakeSingleton(this, ref _instance);
        }


        private void Start()
        {
            textWriter = GetComponent<TextWriter>();
            responseHandler = GetComponent<ResponseHandler>();

            CloseDialogue();
        }

        /// <summary>
        /// Starts a given dialogue with a given npc
        /// </summary>
        /// <param name="dialogueObject"></param>
        /// <param name="npc"></param>
        public void StartDialogue(DialogueObject dialogueObject, NPC npc = null)
        {
            if (npc != null)
            {
                _currentNPC = npc;
                nameText.text = npc.Name;
            }
         
            IsOpen = true;
            dialogueBox.SetActive(true);
            StartCoroutine(StepThroughDialogue(dialogueObject));
        }

        /// <summary>
        /// Adds responses to the current dialogue (Reply options)
        /// </summary>
        /// <param name="responseEvents"></param>
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
                if(NPCSprite.sprite!=null) NPCSprite.gameObject.SetActive(true);
                

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
            NPCSprite.sprite = emotion switch
            {
                "angry" => _currentNPC.portrait.Angry,
                "happy" => _currentNPC.portrait.Happy,
                "special" => _currentNPC.portrait.Special,
                _ => _currentNPC.portrait.Neutral
            };
        }

        /// <summary>
        /// Closes the current dialogue. Clears elements and hides it.
        /// </summary>
        public void CloseDialogue()
        {
            dialogueBox.SetActive(false);
            NPCSprite.gameObject.SetActive(false);
            dialogueText.text = string.Empty;
            nameText.text = string.Empty;
            IsOpen = false;
        }
    }
}

