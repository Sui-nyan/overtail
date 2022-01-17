using System.Collections;
using Overtail.Battle;
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

        public override IEnumerator OnGreeting(Overtail.Battle.BattleSystem system) => base.OnGreeting(system);
        public override IEnumerator DoTurnLogic(Overtail.Battle.BattleSystem system) => base.DoTurnLogic(system);
        public override IEnumerator OnVictory(Overtail.Battle.BattleSystem system) => base.OnVictory(system);
        public override IEnumerator OnDefeat(Overtail.Battle.BattleSystem system) => base.OnDefeat(system);
        public override IEnumerator OnAttack(Overtail.Battle.BattleSystem system) => base.OnAttack(system);
        public override IEnumerator OnGetAttacked(Overtail.Battle.BattleSystem system) => base.OnGetAttacked(system);
        public override IEnumerator OnFlirt(Overtail.Battle.BattleSystem system) => base.OnFlirt(system);
        public override IEnumerator OnGetFlirted(Overtail.Battle.BattleSystem system) => base.OnGetFlirted(system);
        public override IEnumerator OnBully(Overtail.Battle.BattleSystem system) => base.OnBully(system);
        public override IEnumerator OnGetBullied(Overtail.Battle.BattleSystem system) => base.OnGetBullied(system);
        public override IEnumerator OnItemUse(Overtail.Battle.BattleSystem system, ItemStack itemStack) => base.OnItemUse(system, itemStack);
        public override IEnumerator OnEscape(Overtail.Battle.BattleSystem system) => base.OnEscape(system);
        public override IEnumerator OnOpponentEscapes(Overtail.Battle.BattleSystem system) => base.OnOpponentEscapes(system);
    }
}
