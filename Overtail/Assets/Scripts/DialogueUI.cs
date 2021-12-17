using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private DialogueObject test;

    private TextWriter textWriter;

    private void Start()
    {
        textWriter = GetComponent<TextWriter>();
        CloseDialogue();
        StartDialogue(test);
    }

    public void StartDialogue(DialogueObject dialogueObject)
    {
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        foreach (string dialogue in dialogueObject.Dialogue)
        {
            yield return textWriter.Run(dialogue, textLabel);
            yield return new WaitUntil(() => Input.anyKeyDown);
        }

        CloseDialogue();
    }

    private void CloseDialogue()
    {
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
    }
}
