using System.Collections;
using UnityEngine;
namespace Overtail.Battle
{
    public class VictoryState : State
    {
        public VictoryState(BattleSystem system) : base(system) { }

        public override IEnumerator Start()
        {
            _system.textBox.text = "You won.";
            yield return new WaitForSeconds(1f);
            _system.Exit();
        }
    }
}