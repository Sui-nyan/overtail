using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overtail.Deprecated
{
    public class _PersistentPlayerData : ScriptableObject
    {
        public _Template template;

        public int level;
        public int currentHealth;
        public _Vec3 position;

        public _Equipment equipment;
        public List<_Buff> buffs = new List<_Buff>();
    }

    [System.Serializable]
    public class _Vec3
    {
        public float x = 0;
        public float y = 0;
        public float z = 0;
    }

}