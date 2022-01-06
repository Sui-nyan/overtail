using System.Collections;
using UnityEngine;

namespace Overtail.Battle
{
    public class DefeatState : State
    {
        public DefeatState(BattleSystem system) : base(system) { }

        public override IEnumerator Start()
        {
            yield return _system.StartCoroutine(_system.Enemy.OnVictory(_system));
            yield return _system.StartCoroutine(_system.Player.OnDefeat(_system));
            yield return _system.GUI.StartDialogue("U deaded");
            yield return _system.GUI.StartDialogue("You have been defeated.*");
            _system.ExitBattle();
        }
    }
}