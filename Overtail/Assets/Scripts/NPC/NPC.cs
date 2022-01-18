using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Overtail.NPCs
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
        [SerializeField] private Sprite _neutral;
        [SerializeField] private Sprite _angry;
        [SerializeField] private Sprite _happy;
        [SerializeField] private Sprite _special;

        public Sprite Neutral => _neutral;
        public Sprite Angry => _angry != null ? _angry : _neutral;
        public Sprite Happy => _happy != null ? _happy : _neutral;
        public Sprite Special => _special != null ? _special : _neutral;
    }
}