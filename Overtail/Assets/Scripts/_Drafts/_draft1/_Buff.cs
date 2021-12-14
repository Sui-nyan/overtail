using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overtail.Deprecated
{
    [System.Serializable]
    public class _Buff
    {
        public _StatType type;
        public float value;
        public bool multiplicative;

        public int duration;
        public bool permanent;
    }

    //[System.Serializable]
    public enum _StatType { ATK, DEF, HP }
}