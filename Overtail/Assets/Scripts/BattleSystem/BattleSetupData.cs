using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

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
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject enemyPrefab;
        public GameObject PlayerPrefab => playerPrefab;
        public GameObject EnemyPrefab => enemyPrefab;        
    }
}