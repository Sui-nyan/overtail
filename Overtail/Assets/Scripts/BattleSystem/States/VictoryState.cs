using System.Collections;
using UnityEngine;
namespace Overtail.Battle
{
    public class VictoryState : State
    {
        public VictoryState(BattleSystem system) : base(system) { }

        public override IEnumerator Start()
        {
            yield return _system.StartCoroutine(_system.Enemy.OnDefeat(_system));
            yield return new WaitUntil(() => _system.IsIdle);

            _system.GUI.QueueMessage($"{_system.Enemy} has been defeated. You win.");
            yield return new WaitUntil(() => _system.IsIdle);

            _system.Exit();
        }
    }
}