using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;

    public bool IsOpen { get; private set; }
    public TMP_Text nameText;
    public TMP_Text dialogueText;

    private TextWriter textWriter;

    private void Start()
    {
        textWriter = GetComponent<TextWriter>();
        CloseDialogue();

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
        foreach (string dialogue in dialogueObject.Dialogue)
        {
            yield return textWriter.Run(dialogue, dialogueText);
            yield return new WaitUntil(() => Input.anyKeyDown);
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
