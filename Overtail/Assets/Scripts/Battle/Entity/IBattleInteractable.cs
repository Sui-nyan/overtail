using System.Collections;

namespace Overtail.Battle.Entity
{
    /// <summary>
    /// Redundant?
    /// </summary>
    public interface IBattleInteractable
    {
        string Name { get; }
        int HP { get; set; }
        int MaxHP { get; }
        int Attack { get; }
        int Defense { get; }

        IEnumerator DoTurnLogic(Overtail.Battle.BattleSystem system);
        void AddStatusEffect(StatusEffect buff);
    }
}