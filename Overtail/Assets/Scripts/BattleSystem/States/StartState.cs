using System.Collections;
using UnityEngine;

namespace Overtail.Battle
{
    public class StartState : State
    {
        public StartState(BattleSystem system) : base(system) { }
        public override IEnumerator Start()
        {
            _system.GUI.UpdateHUD();
            _system.GUI.SetText($"A wild {_system.Enemy.Name} appears.");

            yield return new WaitForSeconds(2f);

            _system.SetState(new PlayerTurnState(_system));
        }
    }
}