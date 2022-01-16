using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overtail.NPC

{
    [CreateAssetMenu(menuName = "NPC/EmotionEntry")]

    [System.Serializable]
    public class EmotionEntry : ScriptableObject
    {
        [SerializeField] public string id;

        [SerializeField] private Sprite[] sprites;

        public string ID => id;

        public Sprite[] Sprites => sprites;
    }

}
