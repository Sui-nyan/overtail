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
            system.textBox.text = "A wild " + system.enemyUnit.Name + " appears.";

            system.enemyUnit.TakeDamage(-system.enemyUnit.MaxHP);
            system.playerUnit.TakeDamage(-system.playerUnit.MaxHP);

            system.UpdateHUD();
            yield return new WaitForSeconds(2f);
            // Replace with input

            system.SetState(new PlayerTurnState(system));
        }
    }
}