using System.Collections;
using UnityEngine;
namespace Overtail.Battle
{
    public class VictoryState : State
    {
        public VictoryState(BattleSystem system) : base(system) { }

        public override IEnumerator Start()
        {
            system.GUI.SetText($"{system.Enemy} has been defeated. You win.");
            yield return new WaitForSeconds(1f);

            system.Exit();
        }
    }
}