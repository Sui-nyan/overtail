using System.Collections.Generic;
using Overtail.Battle;
using UnityEngine;


namespace Overtail
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Player Data")]
    [System.Serializable]
    public class PlayerData : ScriptableObject
    {
        // Temporary
        public string Name;
        public int BaseMaxHp, BaseAttack, BaseDefense, Charm, Hp, Level, Experience;
        public List<StatusEffect> StatusEffects;
        public Position Position;

        public PlayerSerializable playerSerializable { get; set; }
    }

    public struct Position
    {
        public float x;
        public float y;
    }
}