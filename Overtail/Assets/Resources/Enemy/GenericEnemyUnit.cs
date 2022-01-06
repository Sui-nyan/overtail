using System;
using System.Collections;
using Overtail.Pending;
using UnityEngine;

namespace Overtail.Battle
{
    /// <inheritdoc />
    public class GenericEnemyUnit : EnemyUnit
    {
        // Overrides
        public override IEnumerator OnGreeting(BattleSystem system)
        {
            yield return system.GUI.StartDialogue($"A wild {Name.ToUpper()} has appeared.");
            yield break;
        }

        public override IEnumerator DoTurnLogic(BattleSystem system)
        {
            // Simply attacks player
            yield return StartCoroutine(OnAttack(system));
            yield return StartCoroutine(system.Player.OnGetAttacked(system));
            yield break;
        }

        public override IEnumerator OnVictory(BattleSystem system)
        {
            yield return system.GUI.StartDialogue($"{Name.ToUpper()} walks off.");
            yield break;
        }

        public override IEnumerator OnDefeat(BattleSystem system)
        {
            yield return system.GUI.StartDialogue($"{Name.ToUpper()} disappears into thin air.");
            yield break;
        }

        public override IEnumerator OnAttack(BattleSystem system)
        {
            yield return system.GUI.StartDialogue($"{this.Name} attacks {system.Player.Name}.");
            yield break;
        }

        public override IEnumerator OnGetAttacked(BattleSystem system)
        {
            var hpBefore = HP;
            this.HP -= Math.Max(system.Player.Attack - this.Defense, 0);

            // Sends this message when HP drops below 50%
            if ((float)hpBefore / MaxHP > 0.5f && (float)HP / MaxHP < 0.5f)
            {
                yield return system.GUI.StartDialogue($"{Name} looks tired.");
            }

            yield break;
        }
        
        public override IEnumerator OnGetFlirted(BattleSystem system)
        {
            if (Affection > 10)
            {
                yield return system.GUI.StartDialogue($"{Name.ToUpper()} is too embarrassed to keep fighting.");
                yield return StartCoroutine(OnEscape(system));
                yield return StartCoroutine(system.Player.OnOpponentEscapes(system));
                yield break;
            }

            Affection += system.Player.Charm;
            yield return system.GUI.StartDialogue($"{Name} is not interested in {system.Player.Name}");
            yield break;
        }

        public override IEnumerator OnGetBullied(BattleSystem system)
        {
            if (Affection < 10)
            {
                yield return system.GUI.StartDialogue($"{Name.ToUpper()} is really irritated by the player.");
                yield return StartCoroutine(OnEscape(system));
                yield return StartCoroutine(system.Player.OnOpponentEscapes(system));
                yield break;
            }

            Affection -= system.Player.Charm;
            yield return system.GUI.StartDialogue($"{Name} ignored {system.Player.Name}...");
        }

        public override IEnumerator OnEscape(BattleSystem system)
        {
            yield return system.GUI.StartDialogue($"{Name.ToUpper()} runs off.");
            yield break;
        }

        public override IEnumerator OnOpponentEscapes(BattleSystem system)
        {
            yield return system.GUI.StartDialogue($"{Name.ToUpper()}'s stare follows {system.Player.Name}");
            yield return system.GUI.AwaitTimeOrConfirm();
            system.Escape();
        }
    }
}