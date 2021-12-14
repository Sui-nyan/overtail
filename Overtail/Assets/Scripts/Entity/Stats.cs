using System.Collections.Generic;
using UnityEngine;

namespace Overtail.Entity
{
    /// <summary>
    /// Combat related stats encapsulated into seperate class
    /// Redundant if we only have too little different stats.
    /// </summary>
    [System.Serializable]
    public class Stats
    {
        public int Attack;
        public int Defense;
        public int MaxHealth;
        public int Health;

        public override string ToString()
        {
            return Attack + " ATK, " + Defense + " DEF, " + Health + "/" + MaxHealth + " HP";
        }
    }

    public enum StatType { Attack, Defense, MaxHealth }
}