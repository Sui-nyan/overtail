using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Overtail.NPC
{
    [DisallowMultipleComponent]
    public class NPC : MonoBehaviour
    {
        [SerializeField] private string _name;
        [SerializeField] private int _affection; // future features

        public Portrait portrait;
        public Sprite _sprite;
        public Sprite BaseSprite => _sprite;
        public string Name => _name;
    }

    [System.Serializable]
    public struct Portrait
    {
        public Sprite neutral;
        public Sprite angry;
        public Sprite happy;
        public Sprite special;
    }
}