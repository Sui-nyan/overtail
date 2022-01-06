using System.Collections;
using UnityEngine;

namespace Overtail.Battle
{
    public class EnemyTurnState : State
    {
        public EnemyTurnState(BattleSystem system) : base(system) { }

        public override IEnumerator Start()
        {
            Debug.Log("EnemyTurn:Start");

            yield return _system.StartCoroutine(_system.Enemy.DoTurnLogic(_system));

            if (_system.Player.HP <= 0)
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