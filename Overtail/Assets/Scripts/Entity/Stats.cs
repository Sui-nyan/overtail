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
        public int ATK;
        public int DEF;
        public int HP;
        public int currentHP;
    }

    public enum StatType { ATK, DEF, HP }
}