﻿using System.Collections;
using UnityEngine;

namespace Overtail.Battle
{
    public class EnemyTurnState : State
    {
        public EnemyTurnState(BattleSystem system) : base(system) { }

        public override IEnumerator Start()
        {
            _system.GUI.SetText("Opponent is choosing an action.");
            yield return _system.StartCoroutine(_system.GUI.WaitOrConfirm());

            yield return _system.StartCoroutine(_system.Enemy.DoTurn(_system, _system.Player));
            yield return _system.StartCoroutine(_system.GUI.WaitOrConfirm());

            if (_system.Player.HP <= 0)
            {
                _system.SetState(new DefeatState(_system));
            }
            else
            {
                _system.SetState(new PlayerTurnState(_system));
            }
        }
    }
}