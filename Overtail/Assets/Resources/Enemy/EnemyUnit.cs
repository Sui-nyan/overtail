using System.Collections;
using Overtail.Pending;

namespace Overtail.Battle
{
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

        public override IEnumerator GetAttacked(BattleSystem system)
        {
            return base.GetAttacked(system);
        }

        public override IEnumerator OnFlirt(BattleSystem system)
        {
            return base.OnFlirt(system);
        }

        public override IEnumerator GetFlirted(BattleSystem system)
        {
            return base.GetFlirted(system);
        }

        public override IEnumerator OnBully(BattleSystem system)
        {
            return base.OnBully(system);
        }

        public override IEnumerator GetBullied(BattleSystem system)
        {
            return base.GetBullied(system);
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