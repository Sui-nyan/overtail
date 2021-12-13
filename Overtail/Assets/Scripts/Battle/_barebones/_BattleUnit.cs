using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overtail.Deprecated
{
    public class _BattleUnit : MonoBehaviour
    {
        public _Template template;
        public _Gear equipments;
        public List<_Buff> buffs = new List<_Buff>();


        public _Stats currentStats;

        void Start() { } // Initialize sprite and data, needs template and equipment/other
        void GetAction() { } // BattleSystem > command.execute(actor, target)
    }

    [System.Serializable]
    public class _Gear
    {
        public _Headgear head;
        public _Armor armor;
        public _Weapon rightHand;
        public _Weapon leftHand;
    }

    [System.Serializable]
    public class _Stats
    {
        public int level;
        public int ATK;
        public int DEF;
        public int HP;
        public int currentHP;
    }
}