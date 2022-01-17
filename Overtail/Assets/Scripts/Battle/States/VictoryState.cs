using System.Collections;

namespace Overtail.Battle.States
{
    public class VictoryState : State
    {
        public VictoryState(BattleSystem system) : base(system) { }

        public override IEnumerator Start()
        {
            yield return _system.StartCoroutine(_system.Enemy.OnDefeat(_system));
            yield return _system.StartCoroutine(_system.Player.OnVictory(_system));
            yield return _system.GUI.StartDialogue($"{_system.Enemy} has been defeated.");
            _system.Exit();
        }
    }
}
