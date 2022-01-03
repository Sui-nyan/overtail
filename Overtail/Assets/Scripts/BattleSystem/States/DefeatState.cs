using System.Collections;
using UnityEngine;

namespace Overtail.Battle
{
    public class DefeatState : State
    {
        public DefeatState(BattleSystem system) : base(system) { }

        public override IEnumerator Start()
        {
            _system.GUI.SetText("You deaded");
            yield return new WaitForSeconds(0.2f);
            _system.GUI.SetText("You have been defeated.");
            yield return new WaitForSeconds(1f);

            _system.Exit();
        }
    }
}