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
            Debug.Log("PlayerTurn:Start");
            _system.GUI.QueueMessage("Choose an action.");
            yield return new WaitUntil(() => _system.IsIdle);
            _system.GUI.ShowButtons();
            yield break;
        }
        public override IEnumerator Attack()
        {
            yield return _system.StartCoroutine(_system.Player.PreAttack(_system));
            yield return _system.StartCoroutine(_system.Enemy.GetAttacked(_system));
            yield return new WaitUntil(() => _system.IsIdle);
            EndTurn();
        }



        public override IEnumerator Flirt()
        {
            yield return _system.StartCoroutine(_system.Player.PreFlirt(_system));
            yield return _system.StartCoroutine(_system.Enemy.GetFlirted(_system));
            yield return new WaitUntil(() => _system.IsIdle);
            EndTurn();
        }

        public override IEnumerator Bully()
        {
            yield return _system.StartCoroutine(_system.Player.PreBully(_system));
            yield return _system.StartCoroutine(_system.Enemy.GetBullied(_system));
            yield return new WaitUntil(() => _system.IsIdle);
            EndTurn();
        }

        public override IEnumerator UseItem(ItemStack itemStack)
        {
            yield return _system.StartCoroutine(_system.Player.PreItem(_system, itemStack));
            InventoryManager.Instance?.UseItem(itemStack);
            yield return new WaitUntil(() => _system.IsIdle);
            EndTurn();
        }

        private void EndTurn()
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