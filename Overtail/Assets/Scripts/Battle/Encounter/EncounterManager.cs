using System;
using Overtail.Battle.Entity;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using Overtail.Battle;
using Overtail.Items;
using Overtail.PlayerModule;
using Overtail.Util;
using UnityEngine.Tilemaps;
using Random = System.Random;

namespace Overtail.Battle.Encounter
{
    /// <summary>
    /// Class to generate a random encounter and transition into battle.
    /// Attach to any GameObject.
    /// </summary>
    public class EncounterManager : MonoBehaviour{
        private static EncounterManager _instance;
        public static EncounterManager Instance => _instance;

        public PlayerModule.Player Player;
        public BattleSetup setup;

        [SerializeField] private Pedometer _pedometer;

        public int minLevel = 1;
        public int maxLevel = 99;
        public EnemyEncounter[] spawnableEnemies;

        private void Awake()
        {
            MonoBehaviourExtension.MakeSingleton(this, ref _instance, keepAlive:true, destroyOnSceneZero:true);
        }

        private void Start()
        {
            Assign();
            SceneLoader.Instance.OverWorldSceneLoaded += Assign;
        }

        private void OnDestroy()
        {
            SceneLoader.Instance.OverWorldSceneLoaded -= Assign;
        }

        private void Assign()
        {
            Player = FindObjectOfType<Player>();
            var grassTiles = GameObject.Find("Environment")?.GetComponent<Tilemap>();

            // if(_pedometer != null) _pedometer.EventTick -= TryEncounter; //Resubscribe
            // _pedometer = new Pedometer(Player, grassTiles);
            // _pedometer.EventTick += TryEncounter;

            _pedometer.EventTick -= TryEncounter;
            _pedometer.EventTick += TryEncounter;
            _pedometer.SetPlayer(Player);
            _pedometer.SetMap(grassTiles);
        }

        void FixedUpdate()
        {
            _pedometer.FixedUpdate();
        }

        private void TryEncounter(float current, float max)
        {
            bool trigger = current > max;
            if (trigger)
            {
                _pedometer.Reset();

                UnityEngine.Debug.Log("<Random Encounter!>");
                InitiateEncounter(GetRandomEnemy());
            }
        }

        public void InitiateEncounter(EnemyEntity enemyPrefab = null)
        {
            DontDestroyOnLoad(Player);
            setup.player = Player;
            setup.enemy = enemyPrefab;
            UnityEngine.Debug.Log(enemyPrefab.Level + "::" + setup.enemy.Level);

            SceneLoader.LoadCombatScene();
        }

        EnemyEntity GetRandomEnemy()
        {
            var encounter = EnemyEncounter.GetRandom(spawnableEnemies);

            if (encounter.randomizeLevel)
            {
                encounter.enemy.Level = UnityEngine.Random.Range(minLevel, maxLevel);
            }

            return encounter.enemy;
        }
    }
}