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
            system.GUI.QueueMessage($"A wild {Name.ToUpper()} has appeared.");
            yield break;
        }

        public override IEnumerator DoTurnLogic(BattleSystem system)
        {
            // Simply attacks player
            yield return StartCoroutine(OnAttack(system));
            yield return StartCoroutine(system.Player.GetAttacked(system));
            yield break;
        }

        public override IEnumerator OnVictory(BattleSystem system)
        {
            yield return system.GUI.StartWriteText($"{Name.ToUpper()} walks off.");
            yield break;
        }

        public override IEnumerator OnDefeat(BattleSystem system)
        {
            system.GUI.QueueMessage($"{Name.ToUpper()} disappears into thin air.");
            yield break;
        }

        public override IEnumerator OnAttack(BattleSystem system)
        {
            system.GUI.QueueMessage($"{this.Name} attacks {system.Player.Name}.");
            yield break;
        }

        public override IEnumerator GetAttacked(BattleSystem system)
        {
            var hpBefore = HP;
            this.HP -= Math.Max(system.Player.Attack - this.Defense, 0);

            // Sends this message when HP drops below 50%
            if ((float)hpBefore / MaxHP > 0.5f && (float)HP / MaxHP < 0.5f)
            {
                system.GUI.QueueMessage($"{Name} looks tired.");
            }

            yield break;
        }
        
        public override IEnumerator GetFlirted(BattleSystem system)
        {
            if (Affection > 10)
            {
                system.GUI.QueueMessage($"{Name.ToUpper()} is too embarrassed to keep fighting.");
                system.GUI.QueueCoroutine(OnEscape);
                system.GUI.QueueCoroutine(system.Player.OnOpponentEscapes);
                yield break;
            }

            Affection += system.Player.Charm;
            system.GUI.QueueMessage($"{Name} is not interested in {system.Player.Name}");
            yield break;
        }

        public override IEnumerator GetBullied(BattleSystem system)
        {
            if (Affection < 10)
            {
                system.GUI.QueueMessage($"{Name.ToUpper()} is really irritated by the player.");
                system.GUI.QueueCoroutine(OnEscape);
                system.GUI.QueueCoroutine(system.Player.OnOpponentEscapes);
                yield break;
            }

            Affection -= system.Player.Charm;
            system.GUI.QueueMessage($"{Name} ignored {system.Player.Name}...");
        }

        public override IEnumerator OnEscape(BattleSystem system)
        {
            system.GUI.QueueMessage($"{Name.ToUpper()} runs off.");
            yield break;
        }

        public override IEnumerator OnOpponentEscapes(BattleSystem system)
        {
            system.GUI.QueueMessage($"{Name.ToUpper()}'s stare follows {system.Player.Name}");
            yield return system.GUI.AwaitIdle();
            yield return system.GUI.AwaitTimeOrConfirm();
            system.Escape();
        }
    }
}