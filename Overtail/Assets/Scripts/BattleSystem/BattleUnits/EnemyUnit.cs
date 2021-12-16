using Overtail.Items;
using System.Collections;
using UnityEngine;
using Overtail.Battle;

namespace Overtail.Battle
{
    public class EnemyUnit : BattleUnit
    {
        public override IEnumerator DoTurn(BattleSystem system, IBattleInteractable opponent)
        {
            system.GUI.SetText($"{this.Name} attacks {opponent.Name}.");
            yield return new WaitForSeconds(.5f);
            opponent.TakeDamage(this.Attack);
            system.GUI.UpdateHUD();
            yield return "Hello";
        }

        public override IEnumerator InteractedOn(BattleSystem system, IBattleInteractable opponent)
        {
            system.GUI.SetText($"{opponent.Name} is trying to talk to {this.Name}");
            yield return new WaitForSeconds(1f);
            system.GUI.SetText($"{this.Name} doesn't want to talk to you...");
            yield return new WaitForSeconds(1f);

            system.RestartState();
        }
    }

    public class SlimeBattleUnit : EnemyUnit
    {

    }
}