using System.Collections.Generic;
using UnityEngine;
using Overtail.Serializable;
using Overtail.Battle;
// data class => JSON

namespace Overtail
{

    [System.Serializable]
    public class PlayerSerializable
    {
        [Header("General")]
        [SerializeField] private string name;
        [SerializeField] private PlayerStatus status;
        [SerializeField] List<StatusEffect> statusEffects;

        [Header("Overworld")]
        [SerializeField] private Vec3 pos;

       

        // Accessors

        public string Name => name;

        #region Stats
        public int Level
        {
            get => status.level;
            set => status.level = value;
        }
        public int Exp
        {
            get => status.exp;
            set => status.exp = value;
        }
        public int HP
        {
            get => status.hp;
            set => status.hp = value;
        }
        public int MaxHP
        {
            get => status.maxHp;
            set => status.maxHp = value;
        }
        public int Attack
        {
            get => status.attack;
            set => status.attack = value;
        }
        public int Defense
        {
            get => status.defense;
            set => status.defense = value;
        }
        #endregion
        public List<StatusEffect> StatusEffects => statusEffects;

        #region Overworld/Inventory
        public Vector3 Position
        {
            get => new Vector3(pos.x, pos.y, pos.z);
            set => pos = new Vec3(value.x, value.y, value.z);
        }
        #endregion
    }

}

namespace Overtail.Serializable
{
    [System.Serializable]
    class Vec3
    {
        [SerializeField] internal float x = 0;
        [SerializeField] internal float y = 0;
        [SerializeField] internal float z = 0;
        internal Vec3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    [System.Serializable]
    class PlayerStatus
    {
        [SerializeField] internal int level;
        [SerializeField] internal int exp;
        [SerializeField] internal int hp;
        [SerializeField] internal int maxHp;
        [SerializeField] internal int attack;
        [SerializeField] internal int defense;
    }
}