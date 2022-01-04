using System.Collections;

namespace Overtail.Battle
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

        IEnumerator DoTurn(BattleSystem system);
        void InflictStatus(StatusEffect buff);
        void TurnUpdate(int turns);
        void TurnUpdate();
    }
}