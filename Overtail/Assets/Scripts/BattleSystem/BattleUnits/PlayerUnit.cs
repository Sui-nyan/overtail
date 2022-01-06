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

        public int Exp { get; set; }
        public int Charm { get; private set; }

        private void Awake()
        {
            Load();
        }

        public void BeforeExit()
        {
            Save();
        }

        private void Load()
        {
            p = persistentData.playerSerializable;

            name = p.Name;
            Name = p.Name;
            Level = p.Level;

            SetBaseStats(p.MaxHP, p.Attack, p.Defense);
            HP = p.HP;

            statusEffects.Clear();
            p.StatusEffects.ForEach(s => statusEffects.Add(new StatusEffect(s)));
        }
        private void Save()
        {
            p.Exp = 0;// EXP++

            p.Level = Level;
            p.HP = HP;
            p.MaxHP = MaxHP;
            p.Attack = Attack;
            p.Defense = Defense;

            p.StatusEffects.Clear();
            statusEffects.ForEach(s => p.StatusEffects.Add(new StatusEffect(s)));
        }

        public override IEnumerator OnAttack(BattleSystem system)
        {
            yield break;
        }

        public override IEnumerator OnFlirt(BattleSystem system)
        {
            // Placeholder
            system.GUI.QueueMessage("You found some corny lyrics");
            system.GUI.QueueMessage("Can you hear me?");
            system.GUI.QueueMessage("Im talking to you");
            system.GUI.QueueMessage("Across the water");
            system.GUI.QueueMessage("Across the deap, blue, ocean");
            system.GUI.QueueMessage("Under the open sky");
            system.GUI.QueueMessage("Oh my, baby im tryin.");

            yield break;
        }

        public override IEnumerator OnBully(BattleSystem system)
        {
            system.GUI.QueueMessage($"{Name} is trying to sh*t-talk {system.Enemy.Name}");
            yield return new WaitUntil(() => system.IsIdle);
            yield return new WaitForSeconds(1f);
        }
        public override IEnumerator OnItemUse(BattleSystem system, ItemStack itemStack)
        { 
            yield break;
        }



        public override IEnumerator OnVictory(BattleSystem system)
        {
            p.Exp += 0; // system.Enemy.EXPValue

            // CheckForLevelUp();

            yield break;
        }
    }


}
