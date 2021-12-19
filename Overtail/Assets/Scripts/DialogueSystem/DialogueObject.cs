using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]

[System.Serializable]
public class DialogueObject : ScriptableObject //Objects to store Dialogues
{
    [SerializeField] [TextArea] private string[] dialogue;
    [SerializeField] Response[] responses;

    public string[] Dialogue => dialogue;

    public string NPCName;

    public Response[] Responses => responses;

    public bool HasResponses => Responses != null && Responses.Length > 0;
}
