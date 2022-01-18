using Overtail.Battle.Entity;
using Overtail.PlayerModule;
using UnityEngine;

namespace Overtail.Battle
{
    /// <summary>
    /// Persistent data to pass from overworld to battle.
    /// Might become redundant if we load scenes additively or keep EncounterSystem loaded.
    /// </summary>

    [CreateAssetMenu(fileName = "BattleSetup", menuName = "ScriptableObject/Battle Setup")]
    // Data class containing information to setup the battle scene
    public class BattleSetup : ScriptableObject
    {
        public EnemyEntity enemy;
        public Player player;
    }
}
