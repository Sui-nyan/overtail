using System.Collections;
using Overtail.Pending;
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
            _system.GUI.UpdateHud(); // TODO remove

            _system.GUI.QueueMessage($"{_system.Player.Name} attacked {_system.Enemy.Name}.");  
            yield return new WaitUntil(() => _system.IsIdle);
        }

        public override IEnumerator Flirt()
        {
            yield return _system.StartCoroutine(_system.Enemy.GetFlirted(_system, _system.Player));
        }

        public override IEnumerator Bully()
        {
            yield return _system.StartCoroutine(_system.Enemy.GetBullied(_system, _system.Player));
        }

        public override IEnumerator UseItem(ItemStack itemStack)
        {
            yield return _system.StartCoroutine(_system.Player.Magic());
            InventoryManager.Instance?.UseItem(itemStack);
            _system.GUI.QueueMessage($"{_system.Player.Name} used {itemStack?.Item?.Name}.");

            yield return new WaitUntil(() => _system.IsIdle);
            Debug.Log("Whoops");
            EndState();
        }

        private void EndState()
        {
            if (_system.Enemy.HP <= 0)
            {
                _system.SetState(new VictoryState(_system));
            }
            else
            {
                _system.SetState(new EnemyTurnState(_system));
            }
        }

        public override IEnumerator Escape()
        {
            _system.GUI.QueueMessage("You fled the battle, coward!");

            yield return new WaitUntil(() => _system.IsIdle);
            _system.Exit();
        }
        public override IEnumerator CleanUp()
        {
            _system.GUI.HideButtons();
            _system.Player.TurnUpdate();

            yield break;
        }
    }
}