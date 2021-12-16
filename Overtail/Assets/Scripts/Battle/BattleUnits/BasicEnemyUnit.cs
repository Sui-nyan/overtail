using Overtail.Items;
using System.Collections;
using UnityEngine;
namespace Overtail.Battle
{
    public class BasicEnemyUnit : BattleUnit
    {
        public override IEnumerator DoTurn(BattleSystem system, IBattleInteractable opponent)
        {
            system.textBox.text = $"{this.Name} attacks {opponent.Name}.";
            yield return new WaitForSeconds(.5f);
            opponent.TakeDamage(this.Attack);
            system.UpdateHUD();
            yield return "Hello";
        }

        public override IEnumerator InteractedOn(BattleSystem system, IBattleInteractable opponent)
        {
            system.textBox.text = $"{opponent.Name} is trying to talk to {this.Name}";
            yield return new WaitForSeconds(1f);
            system.textBox.text = $"{this.Name} doesn't want to talk to you...";
            yield return new WaitForSeconds(1f);

            system.RestartState();
        }
    }
}