using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]

[System.Serializable]
public class DialogueObject : ScriptableObject
{
    [SerializeField] [TextArea] private string[] dialogue;
    [SerializeField] Response[] responses;

    public string[] Dialogue => dialogue;

    public string NPCName;

    private Response[] Responses => responses;
}
