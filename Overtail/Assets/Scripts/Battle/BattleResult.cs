using System.Collections.Generic;
using Overtail.Battle.Entity;
using Overtail.Battle;
using UnityEngine;


namespace Overtail.Battle
{
    [CreateAssetMenu(fileName = "BattleResult", menuName = "ScriptableObject/Battle Result")]
    [System.Serializable]
    public class BattleResult : ScriptableObject
    {
        // Temporary
        public int Hp, Level, Experience;
        public List<StatusEffect> StatusEffects;
        public Position Position;
    }

    [System.Serializable]
    public struct Position
    {
        public float x;
        public float y;
    }
}