using System.Collections;

namespace Overtail.Battle
{
    /// <summary>
    /// Redundant?
    /// </summary>
    public interface IBattleInteractable
    {
        string Name { get; }
        int HP { get; }
        int MaxHP { get; }
        int Attack { get; }
        int Defense { get; }

        void TakeDamage(int damage);
        IEnumerator DoTurn(BattleSystem system, IBattleInteractable opponent);
        void InflictStatus(StatusEffect buff);
        void TurnUpdate(int turns);
        void TurnUpdate();
    }
}