using System.Collections.Generic;
using UnityEngine;


namespace Overtail.Battle
{
    /// <summary>
    /// Persistent data to pass from overworld to battle.
    /// Might become redundant if we load scenes additively or keep EncounterSystem loaded.
    /// </summary>

    [CreateAssetMenu(fileName = "BattleData", menuName = "ScriptableObject/Battle Data")]
    // Data class containing information to setup the battle scene
    public class BattleSetupData : ScriptableObject
    {
        [Header("Debug Only")]
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private GameObject _enemyPrefab;
        public GameObject PlayerPrefab => _playerPrefab;
        public GameObject EnemyPrefab => _enemyPrefab;
    }
}