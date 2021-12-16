using UnityEngine;
using Overtail.Entity;

namespace Overtail.Items
{
    /// <summary>
    /// Equpment definition.
    /// </summary>
    /// 
    [System.Serializable]
    public abstract class Equipment : Item
    {
        [SerializeField]
        private int attack;
        public int Attack => attack;

        [SerializeField]
        private int defense;
        public int Defense => defense;
    }

    [System.Serializable] public class Weapon : Equipment { }
    [System.Serializable] public class HeadEquipment : Equipment { }
    [System.Serializable] public class BodyEquipment : Equipment { }
}