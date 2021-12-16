using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Overtail.Items;
using Overtail.Battle;

namespace Overtail.Entity
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "BattleSystem/Create Persistent Player Data (Unique)")]
    public class PersistentPlayerData : ScriptableObject
    {
        public EntityTemplate template;

        public int level;
        public int currentHealth;
        [SerializeField] private Vec3 position;

        public EquipmentSet equipment;
        public List<StatusEffect> buffs = new List<StatusEffect>();

        public Vector3 Position
        {
            get => new Vector3(position.x, position.y, position.z);
            set => position = new Vec3(value.x, value.y, value.z);
        }
    }


    [System.Serializable]
    internal class Vec3
    {
        internal float x = 0;
        internal float y = 0;
        internal float z = 0;
        internal Vec3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}