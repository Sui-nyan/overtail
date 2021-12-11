using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterAI", menuName = "Monster/Create AI (Behavior)")]
public class MonsterAI : ScriptableObject
{
    public Interaction interactions;
}

public enum InteractionType { BULLY, FLIRT }


[System.Serializable]
public class Interaction
{
    public List<Response> bullyReactions = new List<Response>();
    public List<Response> flirstReactons = new List<Response>();
}


[System.Serializable]
public class Response
{
    [TextArea] public string reply;

    /**
     * Executes Response
     * Return true if successful - otherwise false
     */
    public virtual bool Execute() {
        return false;
    }
}

