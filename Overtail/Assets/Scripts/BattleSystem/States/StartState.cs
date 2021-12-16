using System.Collections;
using UnityEngine;

namespace Overtail.Battle
{
    public class StartState : State
    {
        public StartState(BattleSystem system) : base(system) { }
        public override IEnumerator Start()
        {
            system.GUI.UpdateHUD();
            system.GUI.SetText($"A wild {system.Enemy.Name} appears.");

            yield return new WaitForSeconds(2f);

            system.SetState(new PlayerTurnState(system));
        }
    }
}