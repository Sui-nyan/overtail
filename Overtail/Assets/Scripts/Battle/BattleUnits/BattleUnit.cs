using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Overtail.Items;
using Overtail.Entity;

namespace Overtail.Battle
{
    /// <summary>
    /// Main class for characters participating in combat.
    /// Main Interface to the battle system and vice versa.
    /// </summary>
    public class BattleUnit : MonoBehaviour
    {
        [SerializeField] private EntityTemplate baseUnit;
        [SerializeField] private EquipmentSet equipment;
        [SerializeField] private List<Buff> buffs = new List<Buff>();

        [SerializeField] int level;
        [SerializeField] private Stats currentStats;

        public string Name { get => baseUnit.Name; }
        public int Level { get => level; private set => level = value; }
        public int Attack { get => currentStats.Attack; private set => currentStats.Attack = value; }
        public int Defense { get => currentStats.Defense; private set => currentStats.Defense = value; }
        public int MaxHealth { get => currentStats.MaxHealth; private set => currentStats.MaxHealth = value; }
        public int Health { get => currentStats.Health; private set => currentStats.Health = value; }


        void Start()
        {
        }

        public void Setup()
        {
            //gameObject.GetComponent<SpriteRenderer>().sprite = baseUnit.Sprite;
            RecalculateStats();
        }

        public void RecalculateStats()
        {
            currentStats = Util.CalculateStats(Level,
                                                baseUnit.BaseStats,
                                                equipment,
                                                buffs);
        }

        void GetAction() { } // BattleSystem > command.execute(actor,target)

        internal void TakeDamage(BattleUnit src)
        {
            this.Health = System.Math.Max(this.Health - System.Math.Max(src.Attack - this.Defense, 0), 0);
        }

        internal void Heal()
        {
            this.Health = this.MaxHealth;
        }
    }
}