using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Overtail.Items;
using Overtail.Battle;

namespace Overtail.Entity
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "BattleSystem/Create Persistent Player Data (Unique)")]
    [System.Serializable]
    public class PersistentPlayerData : ScriptableObject
    {

        public Overtail.Player.PlayerSerializable playerData;
/*        public EntityTemplate template;

        public int level;
        public int currentHealth;
        [SerializeField] private Vec3 position;

        public List<StatusEffect> buffs = new List<StatusEffect>();

        public Vector3 Position
        {
            get => new Vector3(position.x, position.y, position.z);
            set => position = new Vec3(value.x, value.y, value.z);
        }*/
    }



}