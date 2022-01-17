using System.Collections;
using Overtail.Items;

namespace Overtail.Battle.Entity
{
    /// <summary>
    /// Base combat related enemy class.
    /// Derive from this class for any monsters or use <see cref="GenericEnemyEntity"/>.
    /// </summary>
    public class EnemyEntity : BattleEntity
    {
        public int Affection { get; protected set; }

        public override IEnumerator OnGreeting(BattleSystem system) => base.OnGreeting(system);
        public override IEnumerator DoTurnLogic(BattleSystem system) => base.DoTurnLogic(system);
        public override IEnumerator OnVictory(BattleSystem system) => base.OnVictory(system);
        public override IEnumerator OnDefeat(BattleSystem system) => base.OnDefeat(system);
        public override IEnumerator OnAttack(BattleSystem system) => base.OnAttack(system);
        public override IEnumerator OnGetAttacked(BattleSystem system) => base.OnGetAttacked(system);
        public override IEnumerator OnFlirt(BattleSystem system) => base.OnFlirt(system);
        public override IEnumerator OnGetFlirted(BattleSystem system) => base.OnGetFlirted(system);
        public override IEnumerator OnBully(BattleSystem system) => base.OnBully(system);
        public override IEnumerator OnGetBullied(BattleSystem system) => base.OnGetBullied(system);
        public override IEnumerator OnItemUse(BattleSystem system, ItemStack itemStack) => base.OnItemUse(system, itemStack);
        public override IEnumerator OnEscape(BattleSystem system) => base.OnEscape(system);
        public override IEnumerator OnOpponentEscapes(BattleSystem system) => base.OnOpponentEscapes(system);
    }
}
