using System.Collections;
using System.Collections.Generic;
using Overtail.Items;
using Overtail.PlayerModule;
using UnityEngine;

namespace Overtail.Battle.Entity
{
    public class PlayerEntity : BattleEntity, IItemInteractor
    {
        private Player _player;

        public int Charm { get; private set; }
        public int ExpLevelUp { get; private set; }
        public float ExpProgress
        {
            get
            {
                var expAboveCurrent = (Experience - StatComponent.ExpLevelUp(Level - 1));
                var expCurrentToNext = StatComponent.ExpLevelUp(Level) - StatComponent.ExpLevelUp(Level - 1);
                return expAboveCurrent / (float)expCurrentToNext;
            }
        }

        public void Load(Player p)
        {
            _player = p;

            var s = p.GetComponent<StatComponent>();

            SetBaseStats(s.MaxHP, s.Attack, s.Defense);

            Name = p.Name;

            Level = s.Level;
            Experience = s.Experience;
            ExpLevelUp = s.ExpLevelUp();

            HP = s.HP;
            Charm = s.Charm;
        }

        public void Save(Player p)
        {
            var player = p.GetComponent<StatComponent>();
            player.Level = Level;
            player.Experience = Experience;
            player.StatusEffects = new List<StatusEffect>(statusEffects);
            player.HP = HP;
        }

        public void Save(BattleResult data)
        {
            data.Level = Level;
            data.Experience = Experience;
            data.StatusEffects = new List<StatusEffect>(statusEffects);
            data.Hp = HP;

            var pos = _player.GetComponent<Rigidbody2D>().position;

            data.Position.x = pos.x;
            data.Position.y = pos.y;
        }

        public void LevelUp()
        {
            var s = _player.GetComponent<StatComponent>();

            MaxHP = s.ScaledMaxHP(Level);
            Attack = s.ScaledAttack(Level);
            Defense = s.ScaledDefense(Level);
            ExpLevelUp = StatComponent.ExpLevelUp(Level);
        }

        public override IEnumerator OnAttack(Overtail.Battle.BattleSystem system)
        {
            system.Enemy.HP -= Attack;
            yield break;
        }


        public override IEnumerator OnFlirt(BattleSystem system)
        {
            // Placeholder
            yield return system.GUI.StartDialogue("You found some corny lyrics");
            yield return system.GUI.StartDialogue("Can you hear me?");
            yield return system.GUI.StartDialogue("Im talking to you");
            yield return system.GUI.StartDialogue("Across the water");
            yield return system.GUI.StartDialogue("Across the deep, blue, ocean");
            yield return system.GUI.StartDialogue("Under the open sky");
            yield return system.GUI.StartDialogue("Oh my, baby im tryin.");

            yield break;
        }

        public override IEnumerator OnBully(BattleSystem system)
        {
            yield return system.GUI.StartDialogue($"{Name} is trying to sh*t-talk {system.Enemy.Name}");
        }

        public override IEnumerator OnItemUse(BattleSystem system, ItemStack itemStack)
        {
            yield break;
        }

        public override IEnumerator OnVictory(BattleSystem system)
        {
            var exp = system.Enemy.Experience;

            while (exp > 0)
            {
                if (StatComponent.ExpLevelUp(Level) <= Experience)
                    LevelUp();

                var expDiff = StatComponent.ExpLevelUp(Level) - Experience;
                var added = Mathf.Min(exp, expDiff);

                Experience += expDiff;
                yield return system.GUI.SmoothExpBar(ExpProgress);
            }

            yield break;
        }

        public Item MainHand { get; set; }
        public Item OffHand { get; set; }
        public void Heal(int hp)
        {
            HP += hp;
            Debug.Log("HEAL TO " + HP);
        }

        public void AddStatus(StatusEffect newEffect)
        {
            return;
        }
    }
}
