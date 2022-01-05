using System.Collections;
using UnityEngine;

namespace Overtail.Battle
{
    public class StartState : State
    {
        public StartState(BattleSystem system) : base(system) { }
        public override IEnumerator Start()
        {
            _system.GUI.QueueMessage($"A wild {_system.Enemy.Name} appears.");

            yield return new WaitUntil(() => _system.IsIdle);
            _system.SetState(new PlayerTurnState(_system));
        }
    }
}