using System.Collections;
using UnityEngine;
using Unity;

namespace Overtail.Battle
{
    public class PlayerTurnState : State
    {
        public PlayerTurnState(BattleSystem system) : base(system) { }
        public override IEnumerator Start()
        {
            system.GUI.SetText("Choose an action.");
            system.GUI.ShowButtons();
            yield break;
        }
        public override IEnumerator Attack()
        {
            system.Enemy.TakeDamage(system.Player);
            system.GUI.UpdateHUD();

            system.GUI.SetText($"{system.Player.Name} attacks {system.Enemy.Name}.");
            yield return new WaitForSeconds(1f);

            if (system.Enemy.HP <= 0)
            {
                system.SetState(new VictoryState(system));
            }
            else
            {
                system.SetState(new EnemyTurnState(system));
            }
        }

        public override IEnumerator Interact()
        {
            yield return system.StartCoroutine(system.Enemy.InteractedOn(system, system.Player));
        }

        public override IEnumerator Inventory()
        {
            system.GUI.SetText("You forgot your bag at home.");
            yield return new WaitForSeconds(1f);
            system.Player.OpenInventory();
            system.RestartState();
        }

        public override IEnumerator Escape()
        {
            system.GUI.SetText("You fled the battle, coward!");
            yield return new WaitForSeconds(1f);
            system.Exit();
        }
        public override IEnumerator Stop()
        {
            system.GUI.HideButtons();
            yield break;
        }
    }
}