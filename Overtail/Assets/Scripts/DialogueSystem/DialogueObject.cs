using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]

[System.Serializable]
public class DialogueObject : ScriptableObject //Objects to store Dialogues
{
    [SerializeField] [TextArea] private string[] dialogue; //Input dialogue here
    [SerializeField] Response[] responses; //Responses can be added in this field 

    public string[] Dialogue => dialogue;

    public string NPCName;

    public Response[] Responses => responses;

    public bool HasResponses => Responses != null && Responses.Length > 0; //Returns true, if the Dialogue has any responses
}
