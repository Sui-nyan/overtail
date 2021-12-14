﻿using System.Collections;
using UnityEngine;
namespace Overtail.Battle
{
    public class DefeatState : State
    {
        public DefeatState(BattleSystem system) : base(system) { }

        public override IEnumerator Start()
        {
            _system.textBox.text = "You have been defeated";
            yield return new WaitForSeconds(0.5f);

            _system.Exit();
        }
    }
}