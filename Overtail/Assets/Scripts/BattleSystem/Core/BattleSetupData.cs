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
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject enemyPrefab;
        public GameObject PlayerPrefab => playerPrefab;
        public GameObject EnemyPrefab => enemyPrefab;

        private int _hp;
        private int _level;
        private List<StatusEffect> _statusEffects;
        private int _exp;

        public void SaveFromCombat(PlayerUnit unit)
        {
            _hp = unit.HP;
            _level = unit.Level;
            _exp = unit.Experience;
            _statusEffects = unit.StatusEffects;
        }

        public void LoadToCombat(PlayerUnit unit)
        {
            unit.HP = _hp;
            unit.Level = _level;
            unit.Experience = _exp;
            unit.StatusEffects=_statusEffects;
        }

        public void SaveFromRoaming(Player p)
        {
            
        }

        public void LoadToRoaming(Player p)
        {

        }
    }
}