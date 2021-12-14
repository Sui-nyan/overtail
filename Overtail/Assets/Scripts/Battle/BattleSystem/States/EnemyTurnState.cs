using System.Collections;
using UnityEngine;
namespace Overtail.Battle
{
    public class EnemyTurnState : State
    {
        public EnemyTurnState(BattleSystem system) : base(system)
        {

        }

        public override IEnumerator Start()
        {
            _system.textBox.text = "Opponent is choosing an action.";
            yield return new WaitForSeconds(1f);

            _system.textBox.text = _system.enemyUnit.Name + " attacked you.";

            _system.playerUnit.TakeDamage(_system.enemyUnit);
            _system.UpdateHUD();


            yield return new WaitForSeconds(1f);


            if (_system.playerUnit.Health <= 0)
            {
                _system.SetState(new DefeatState(_system));
            }
            else
            {
                _system.SetState(new PlayerTurnState(_system));
            }

        }
    }
}