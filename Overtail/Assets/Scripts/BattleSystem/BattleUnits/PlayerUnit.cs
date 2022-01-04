using Overtail.Pending;
using System;
using System.Collections;
using UnityEngine;
namespace Overtail.Battle
{
    public class PlayerUnit : BattleUnit
    {
        [SerializeField] private PersistentPlayerData persistentData;
        [Header("Debug")]
        [SerializeField] private PlayerSerializable p;

        private void Start()
        {
            Load();
        }

        public void End()
        {
            Save();
        }

        private void Load()
        {
            p = persistentData.playerSerializable;

            name = p.Name;
            level = p.Level;
            hp = p.HP;
            maxHp = p.MaxHP;
            attack = p.Attack;
            defense = p.Defense;

            statusEffects.Clear();
            p.StatusEffects.ForEach(s => statusEffects.Add(new StatusEffect(s)));
        }

        internal IEnumerator PreAttack(BattleSystem system)
        {
            yield break;
        }

        internal IEnumerator PreFlirt(BattleSystem system)
        {
            // Placeholder
            system.GUI.QueueMessage("You found some corny lyrics");
            system.GUI.QueueMessage("Can you hear me?");
            system.GUI.QueueMessage("Im talking to you");
            system.GUI.QueueMessage("Across the water");
            system.GUI.QueueMessage("Across the deap, blue, ocean");
            system.GUI.QueueMessage("Under the open sky");
            system.GUI.QueueMessage("Oh my, baby im tryin.");

            yield return new WaitUntil(() => system.IsIdle);
        }

        internal IEnumerator PreItem(BattleSystem system, ItemStack itemStack)
        { 
            yield break;
        }

        internal IEnumerator PreBully(BattleSystem system)
        {
            system.GUI.QueueMessage($"{Name} is trying to sh*t-talk {system.Enemy.Name}");
            yield return new WaitUntil(() => system.IsIdle);
            yield return new WaitForSeconds(1f);
        }

        private void Save()
        {
            p.Exp = 0;// EXP++

            p.Level = level;
            p.HP = hp;
            p.MaxHP = maxHp;
            p.Attack = attack;
            p.Defense = defense;

            p.StatusEffects.Clear();
            statusEffects.ForEach(s => p.StatusEffects.Add(new StatusEffect(s)));
        }



        protected override int GetStat(StatType statType)
        {
            return base.GetStat(statType);
        }

    }


}
