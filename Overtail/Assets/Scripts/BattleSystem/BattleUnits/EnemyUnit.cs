using System.Collections;
using UnityEngine;

namespace Overtail.Battle
{
    public class EnemyUnit : BattleUnit
    {
        private int _affection;
        public override IEnumerator DoTurn(BattleSystem system, IBattleInteractable opponent)
        {
            system.GUI.SetText($"{this.Name} attacks {opponent.Name}.");
            yield return new WaitForSeconds(.5f);
            opponent.TakeDamage(this.Attack);
            system.GUI.UpdateHud();
            yield return "Hello";
        }

        public IEnumerator GetBullied(BattleSystem system, PlayerUnit opponent)
        {
            system.GUI.QueueMessage($"{opponent.Name} is trying to talk down on {this.Name}");
            yield return new WaitForSeconds(1f);
            system.GUI.QueueMessage($"{this.Name} is ignoring you...");
            yield return new WaitUntil(() => system.IsIdle);

            system.RestartState();
        }

        public IEnumerator GetFlirted(BattleSystem system, PlayerUnit opponent)
        {
            system.GUI.QueueMessage($"{opponent.Name} is trying to talk to {this.Name}");
            yield return new WaitForSeconds(1f);
            system.GUI.QueueMessage($"{this.Name} doesn't want to talk to you...");
            yield return new WaitUntil(() => system.IsIdle);

            system.RestartState();
        }

        public IEnumerator OnDefeat()
        {
            yield break;
        }


    }

    public class SlimeBattleUnit : EnemyUnit
    {

    }
}