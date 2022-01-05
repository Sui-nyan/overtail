using System.Collections;
using UnityEngine;

namespace Overtail.Battle
{
    public class DefeatState : State
    {
        public DefeatState(BattleSystem system) : base(system) { }

        public override IEnumerator Start()
        {
            _system.GUI.QueueMessage("You deaded");
            _system.GUI.QueueMessage("You have been defeated.");
            yield return new WaitUntil(() => _system.IsIdle);

            _system.Exit();
        }
    }
}