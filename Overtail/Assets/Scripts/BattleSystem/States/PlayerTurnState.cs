using System.Collections;
using UnityEngine;

namespace Overtail.Battle
{
    public class PlayerTurnState : State
    {
        public PlayerTurnState(BattleSystem system) : base(system) { }
        public override IEnumerator Start()
        {
            _system.GUI.SetText("Choose an action.");
            _system.GUI.ShowButtons();
            yield break;
        }
        public override IEnumerator Attack()
        {
            _system.Enemy.TakeDamage(_system.Player);
            _system.GUI.UpdateHUD();

            _system.GUI.SetText($"{_system.Player.Name} attacks {_system.Enemy.Name}.");
            yield return new WaitForSeconds(1f);

            if (_system.Enemy.HP <= 0)
            {
                _system.SetState(new VictoryState(_system));
            }
            else
            {
                _system.SetState(new EnemyTurnState(_system));
            }
        }

        public override IEnumerator Interact()
        {
            yield return _system.StartCoroutine(_system.Enemy.InteractedOn(_system, _system.Player));
        }

        public override IEnumerator Inventory()
        {
            yield return _system.StartCoroutine(_system.Player.Magic());
            yield break;
        }

        public override IEnumerator Escape()
        {
            _system.GUI.SetText("You fled the battle, coward!");
            yield return new WaitForSeconds(1f);
            _system.Exit();
        }
        public override IEnumerator Stop()
        {
            _system.GUI.HideButtons();
            _system.Player.TurnUpdate();
            yield break;
        }
    }
}