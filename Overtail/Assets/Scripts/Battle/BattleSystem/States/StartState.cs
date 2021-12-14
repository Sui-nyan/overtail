using System.Collections;
using UnityEngine;

namespace Overtail.Battle
{
    public class StartState : State
    {
        public StartState(BattleSystem system) : base(system) { }
        public override IEnumerator Start()
        {

            // Instantiating should call respective GameObject.Start()
            _system.textBox.text = "A wild " + _system.enemyUnit.Name + " appears.";

            _system.enemyUnit.Setup();
            _system.playerUnit.Setup();

            _system.enemyUnit.Heal();
            _system.playerUnit.Heal();

            _system.UpdateHUD();
            yield return new WaitForSeconds(2f);
            // Replace with input

            _system.SetState(new PlayerTurnState(_system));
        }
    }
}