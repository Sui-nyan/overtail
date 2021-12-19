using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private DialogueObject test;

    public bool IsOpen { get; private set; }
    public TMP_Text nameText;
    public TMP_Text dialogueText;

    private ResponseHandler responseHandler;
    private TextWriter textWriter;

    private void Start()
    {
        textWriter = GetComponent<TextWriter>();
        responseHandler = GetComponent<ResponseHandler>();

        CloseDialogue();
        StartDialogue(test);

    }

    public void StartDialogue(DialogueObject dialogueObject)
    {
        nameText.text = dialogueObject.NPCName;
        IsOpen = true;
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {

        for(int i = 0; i < dialogueObject.Dialogue.Length; i++)
        {
            string dialogue = dialogueObject.Dialogue[i];
            yield return textWriter.Run(dialogue, dialogueText);

            if(i == dialogueObject.Dialogue.Length -1 && dialogueObject.HasResponses)
            {
                break;
            }

            yield return new WaitUntil(() => Input.anyKeyDown);
        }

        if (dialogueObject.HasResponses)
        {
            responseHandler.showResponses(dialogueObject.Responses);
        }

        CloseDialogue();
    }

    private void CloseDialogue()
    {
        dialogueBox.SetActive(false);
        dialogueText.text = string.Empty;
        nameText.text = string.Empty;
        IsOpen = false;
    }
}
