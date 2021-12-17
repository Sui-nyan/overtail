using UnityEngine;


namespace Overtail.Battle
{
    /// <summary>
    /// Persistent data to pass from overworld to battle.
    /// Might become redundant if we load scenes additively or keep EncounterSystem loaded.
    /// </summary>

    [CreateAssetMenu(fileName = "BattleSetup", menuName = "Encounter/Battle Setup Object")]
    // Data class containing information to setup the battle scene
    public class BattleSetupData : ScriptableObject
    {
        [Header("Debug Only")]
        [SerializeField] private UnityEngine.GameObject playerPrefab;
        [SerializeField] private UnityEngine.GameObject enemyPrefab;
        public UnityEngine.GameObject PlayerPrefab => playerPrefab;
        public UnityEngine.GameObject EnemyPrefab => enemyPrefab;        
    }
}