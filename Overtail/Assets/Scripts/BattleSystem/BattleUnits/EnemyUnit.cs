using System;
using System.Collections;
using UnityEngine;

namespace Overtail.Battle
{
    public class EnemyUnit : BattleUnit
    {
        private int _affection;

        internal virtual IEnumerator OnGreeting(BattleSystem system)
        {
            yield break;
        }
        public override IEnumerator DoTurn(BattleSystem system)
        {
            system.GUI.QueueMessage($"{this.Name} attacks {system.Player.Name}.");

            system.Player.HP -= Math.Max(0, this.Attack - system.Player.Defense);

            yield return new WaitUntil(() => system.IsIdle);
        }

        internal virtual IEnumerator GetAttacked(BattleSystem system)
        {
            var hpBefore = HP;
            bool hpThreshold(float threshold) => hpBefore / MaxHP > threshold && HP / MaxHP < threshold;

            var dmg = Math.Max(system.Player.Attack - this.Defense, 0 );
            this.HP -= dmg;
            Debug.Log($"{Name} took {dmg} Damage");

            if (hpThreshold(0.1f))
            {
                system.GUI.QueueMessage($"{Name} is pleading for mercy.");
            } else if (hpThreshold(0.5f))
            {
                system.GUI.QueueMessage($"{Name} looks tired.");
            }

            yield break;
        }

        internal virtual IEnumerator GetFlirted(BattleSystem system)
        {
            system.GUI.QueueMessage($"{Name} is not interested in {system.Player.Name}");
            yield return new WaitUntil(() => system.IsIdle);
        }

        internal virtual IEnumerator GetBullied(BattleSystem system)
        {
            system.GUI.QueueMessage($"{Name} ignored {system.Player.Name}...");
            yield return new WaitUntil(() => system.IsIdle);
        }
        public override IEnumerator OnDefeat(BattleSystem system)
        {
            system.GUI.QueueMessage($"{Name}: (x x)");
            yield break;
        }


    }
}