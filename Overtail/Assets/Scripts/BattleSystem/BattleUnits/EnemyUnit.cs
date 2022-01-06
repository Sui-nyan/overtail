using System.Collections;
using Overtail.Pending;

namespace Overtail.Battle
{
    /// <summary>
    /// Base combat related enemy class.
    /// Derive from this class for any monsters or use <see cref="GenericEnemyUnit"/>.
    /// </summary>
    public class EnemyUnit : BattleUnit
    {
        public int Affection { get; protected set; }

        public override IEnumerator OnGreeting(BattleSystem system)
        {
            return base.OnGreeting(system);
        }
        public override IEnumerator DoTurnLogic(BattleSystem system)
        {
            return base.DoTurnLogic(system);
        }
        public override IEnumerator OnVictory(BattleSystem system)
        {
            return base.OnVictory(system);
        }
        public override IEnumerator OnDefeat(BattleSystem system)
        {
            return base.OnDefeat(system);
        }
        public override IEnumerator OnAttack(BattleSystem system)
        {
            return base.OnAttack(system);
        }
        public override IEnumerator OnGetAttacked(BattleSystem system)
        {
            return base.OnGetAttacked(system);
        }
        public override IEnumerator OnFlirt(BattleSystem system)
        {
            return base.OnFlirt(system);
        }
        public override IEnumerator OnGetFlirted(BattleSystem system)
        {
            return base.OnGetFlirted(system);
        }
        public override IEnumerator OnBully(BattleSystem system)
        {
            return base.OnBully(system);
        }
        public override IEnumerator OnGetBullied(BattleSystem system)
        {
            return base.OnGetBullied(system);
        }
        public override IEnumerator OnItemUse(BattleSystem system, ItemStack itemStack)
        {
            return base.OnItemUse(system, itemStack);
        }
        public override IEnumerator OnEscape(BattleSystem system)
        {
            return base.OnEscape(system);
        }
        public override IEnumerator OnOpponentEscapes(BattleSystem system)
        {
            return base.OnOpponentEscapes(system);
        }
    }
}