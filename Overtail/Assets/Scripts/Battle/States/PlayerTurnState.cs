using System.Collections;
using Overtail.Items;
using UnityEngine;

namespace Overtail.Battle.States
{
    public class PlayerTurnState : State
    {
        public PlayerTurnState(Battle.BattleSystem system) : base(system) { }
        public override IEnumerator Start()
        {
            UnityEngine.Debug.Log("PlayerTurn:Start");
            _system.GUI.ShowButtons();
            yield return null;
        }
        public override IEnumerator Attack()
        {
            yield return _system.StartCoroutine(_system.Player.OnAttack(_system));
            yield return _system.StartCoroutine(_system.Enemy.OnGetAttacked(_system));
            EndTurn();
        }



        public override IEnumerator Flirt()
        {
            yield return _system.StartCoroutine(_system.Player.OnFlirt(_system));
            yield return _system.StartCoroutine(_system.Enemy.OnGetFlirted(_system));
            EndTurn();
        }

        public override IEnumerator Bully()
        {
            yield return _system.StartCoroutine(_system.Player.OnBully(_system));
            yield return _system.StartCoroutine(_system.Enemy.OnGetBullied(_system));
            EndTurn();
        }

        public override IEnumerator UseItem(ItemStack itemStack)
        {
            Debug.Log("player used item");
            InventoryManager.Instance?.UseItem(itemStack, _system.Player);
            yield return _system.StartCoroutine(_system.Player.OnItemUse(_system, itemStack));
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
            yield return _system.GUI.StartDialogue("You fled the battle, coward!");
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