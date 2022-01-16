using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Overtail.NPC
{
    [System.Serializable]
    public class NPCObject : ScriptableObject
    {
        [SerializeField] public string NPCName;

        public List<EmotionEntry> emotions;

    }
}

