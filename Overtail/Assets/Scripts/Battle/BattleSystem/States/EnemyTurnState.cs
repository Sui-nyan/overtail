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
            system.textBox.text = "Opponent is choosing an action.";
            yield return new WaitForSeconds(1f);

            yield return system.StartCoroutine(system.enemyUnit.DoTurn(system, system.playerUnit));

            yield return new WaitForSeconds(1f);

            if (system.playerUnit.HP <= 0)
            {
                system.SetState(new DefeatState(system));
            }
            else
            {
                system.SetState(new PlayerTurnState(system));
            }
        }
    }
}