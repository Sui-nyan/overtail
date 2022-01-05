using System;
using System.Collections;
using UnityEngine;

namespace Overtail.Battle
{
    public class EnemyUnit : BattleUnit
    {
        private int _affection;

        public override IEnumerator DoTurn(BattleSystem system)
        {
            system.GUI.QueueMessage($"{this.Name} attacks {system.Player.Name}.");

            system.Player.HP -= Math.Max(0, this.Attack - system.Player.Defense);

            yield return new WaitUntil(() => system.IsIdle);
        }

        internal IEnumerator GetAttacked(BattleSystem system)
        {
            var dmg = Math.Max(system.Player.Attack - this.Defense, 0 );
            Debug.Log("Taking " + dmg + " Damage");
            this.HP -= dmg;

            if(HP/MaxHP < 0.1)
            {
                system.GUI.QueueMessage($"{Name}: \"Oh the pain. MERCY\"");
            } else if (HP/MaxHP < 0.5)
            {
                system.GUI.QueueMessage($"{Name} is becomming desperate");
            }

            yield break;
        }

        public IEnumerator GetFlirted(BattleSystem system, PlayerUnit opponent)
        {
            system.GUI.QueueMessage($"{Name} doesn't want to talk to you...");
            yield return new WaitUntil(() => system.IsIdle);
        }

        public IEnumerator GetBullied(BattleSystem system, PlayerUnit opponent)
        {
            system.GUI.QueueMessage($"{Name} ignored {system.Player.Name}...");
            yield return new WaitUntil(() => system.IsIdle);
        }
        public IEnumerator OnDefeat(BattleSystem system)
        {
            system.GUI.QueueMessage($"{Name}: (x-x)");
            yield break;
        }


    }

    public class SlimeBattleUnit : EnemyUnit
    {

    }
}