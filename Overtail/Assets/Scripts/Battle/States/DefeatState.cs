using System.Collections;

namespace Overtail.Battle.States
{
    public class DefeatState : State
    {
        public DefeatState(Battle.BattleSystem system) : base(system) { }

        public override IEnumerator Start()
        {
            yield return _system.StartCoroutine(_system.Enemy.OnVictory(_system));
            yield return _system.StartCoroutine(_system.Player.OnDefeat(_system));
            yield return _system.GUI.StartDialogue("U deaded");
            yield return _system.GUI.StartDialogue("You have been defeated.*");
            _system.Exit();
        }
    }
}