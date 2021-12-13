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
        public int ATK { get => currentStats.ATK; private set => currentStats.ATK = value; }
        public int DEF { get => currentStats.DEF; private set => currentStats.DEF = value; }
        public int HP { get => currentStats.HP; private set => currentStats.HP = value; }
        public int CurrentHP { get => currentStats.currentHP; private set => currentStats.currentHP = value; }


        private void Start()
        {
            Setup();
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
    }
}