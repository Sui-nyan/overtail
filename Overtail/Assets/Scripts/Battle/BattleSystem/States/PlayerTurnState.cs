using System.Collections;
using UnityEngine;
using Unity;

namespace Overtail.Battle
{
    public class PlayerTurnState : State
    {
        public PlayerTurnState(BattleSystem system) : base(system) { }
        public override IEnumerator Start()
        {
            system.textBox.text = "Choose an action.";
            system.ShowButtons();
            yield break;
        }
        public override IEnumerator Attack()
        {
            system.Lock();
            system.enemyUnit.TakeDamage(system.playerUnit);
            system.UpdateHUD();
            system.textBox.text = "You attacked " + system.enemyUnit.Name + ".";

            
            yield return new WaitForSeconds(1f);

            system.Unlock();
            if (system.enemyUnit.HP <= 0)
            {
                system.SetState(new VictoryState(system));
            }
            else
            {
                system.SetState(new EnemyTurnState(system));
            }
        }

        public override IEnumerator Interact()
        {
            system.Lock();
            yield return system.StartCoroutine(system.enemyUnit.InteractedOn(system, system.playerUnit));
            system.Unlock();
        }

        public override IEnumerator Inventory()
        {
            system.textBox.text = "You forgot your bag at home.";
            yield return new WaitForSeconds(1f);
        }

        public override IEnumerator Escape()
        {

            system.textBox.text = "You fled the battle, coward!";
            yield return new WaitForSeconds(1f);
            system.Exit();
        }
        public override IEnumerator Stop()
        {
            system.HideButtons();
            yield break;
        }
    }
}