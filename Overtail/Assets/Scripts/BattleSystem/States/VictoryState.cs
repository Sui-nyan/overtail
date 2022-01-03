using System.Collections;
using UnityEngine;
namespace Overtail.Battle
{
    public class VictoryState : State
    {
        public VictoryState(BattleSystem system) : base(system) { }

        public override IEnumerator Start()
        {
            _system.GUI.SetText($"{_system.Enemy} has been defeated. You win.");
            yield return new WaitForSeconds(1f);

            _system.Exit();
        }
    }
}