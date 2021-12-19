using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Response //Object to store responses
{
    [SerializeField] private string responseText; //Response
    [SerializeField] private DialogueObject dialogueObject; //Dialogue that follows the response

    public string ResponseText => responseText;

    public DialogueObject DialogueObject => dialogueObject;
}
