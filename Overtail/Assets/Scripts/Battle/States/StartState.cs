using System.Collections;

namespace Overtail.Battle.States
{
    public class StartState : State
    {
        public StartState(Battle.BattleSystem system) : base(system) { }
        public override IEnumerator Start()
        {
            yield return _system.GUI.StartDialogue($"A wild {_system.Enemy.Name} appears.");
            yield return _system.StartCoroutine(_system.Player.OnGreeting(_system));
            yield return _system.StartCoroutine(_system.Enemy.OnGreeting(_system));
            _system.SetState(new PlayerTurnState(_system));
        }
    }
}