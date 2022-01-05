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
            _system.GUI.QueueMessage("Opponent is choosing an action.");
            yield return new WaitUntil(() => _system.IsIdle);

            yield return _system.StartCoroutine(_system.Enemy.DoTurn(_system));
            yield return new WaitUntil(() => _system.IsIdle);

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