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
            _system.textBox.text = "Choose an action.";
            _system.ShowButtons();
            yield break;
        }
        public override IEnumerator Attack()
        {
            _system.enemyUnit.TakeDamage(_system.playerUnit);
            _system.UpdateHUD();
            _system.textBox.text = "You attacked " + _system.enemyUnit.Name + ".";

            yield return new WaitForSeconds(1f);

            if (_system.enemyUnit.Health <= 0)
            {
                _system.SetState(new VictoryState(_system));
            }
            else
            {
                _system.SetState(new EnemyTurnState(_system));
            }
        }

        public override IEnumerator Interact()
        {
            _system.textBox.text = "The enemy doesn't want to interact with you.";
            yield return new WaitForSeconds(1f);
        }

        public override IEnumerator Inventory()
        {
            _system.textBox.text = "You forgot your bag at home.";
            yield return new WaitForSeconds(1f);
        }

        public override IEnumerator Escape()
        {

            _system.textBox.text = "You fled the battle, coward!";
            yield return new WaitForSeconds(1f);
            _system.Exit();
        }
        public override IEnumerator Stop()
        {
            _system.HideButtons();
            yield break;
        }
    }
}