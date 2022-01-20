using System.Collections;
using Overtail.Battle.Entity;
using Overtail.Battle.States;
using Overtail.PlayerModule;
using UnityEngine;
using UnityEngine.SceneManagement;
using Overtail.Battle.UI;

namespace Overtail.Battle
{
    /// <summary>
    /// Overarching system for Battle Scenes.
    /// Logical main entry point. Sets up the battle, starts it and manages the state (State machine).
    /// Add component to some gameObject
    /// Credits to Brackeys - https://www.youtube.com/watch?v=_1pz_ohupPs 
    /// </summary>
    public class BattleSystem : StateMachine
    {
        [Header("TransferedData")]
        [SerializeField] private BattleSetup setup;
        [SerializeField] private BattleResult result;

        [SerializeField] private Transform playerStation;
        [SerializeField] private Transform enemyStation;

        [SerializeField] private PlayerEntity playerEntity;
        [SerializeField] private EnemyEntity enemyEntity;

        private BattleGUI _gui;
        public BattleGUI GUI => _gui;
        public PlayerEntity Player => playerEntity;
        public EnemyEntity Enemy => enemyEntity;

        void Awake()
        {
            _gui = FindObjectOfType<BattleGUI>();
            if (_gui is null) Debug.LogError("Failed to initialize GUI");

            var playerPrefab = Resources.Load<PlayerEntity>("Prefabs/PlayerEntity")?.gameObject;
            if (playerPrefab is null) Debug.LogError($"No player prefab found{playerPrefab}");

            playerEntity = Instantiate(playerPrefab, playerStation).GetComponent<PlayerEntity>();

            var enemyGo = Instantiate(setup.enemy.gameObject, enemyStation);
            enemyEntity = enemyGo.GetComponent<EnemyEntity>();
        }

        void Start()
        {
            var p = FindObjectOfType<Player>();
            // playerEntity.Load(_setup.player);
            playerEntity.Load(p);

            GUI.Setup(this);
            SetState(new StartState(this));
        }

        public void Exit()
        {
            var p = FindObjectOfType<Player>();
            // playerEntity.Save(result);
            playerEntity.Save(p);
            // Destroy(setup.player.gameObject);
            StartCoroutine(TransitionOut());
        }

        public void EscapeBattle()
        {
            // Anything different?
            Exit();
        }

        private IEnumerator TransitionOut()
        {
            // TODO Transition or whatever
            SceneManager.LoadScene("OverWorldScene");
            yield break;
        }

        public void OnAttackButton()
        {
            StartCoroutine(_state.Attack());
        }
        public void OnInteractButton()
        {
            // Open interaction >
            // a) Flirt
            // b) Bully
            GUI.InteractionSubMenu(() => StartCoroutine(_state.Flirt()), () => StartCoroutine(_state.Bully()));
        }
        public void OnInventoryButton()
        {
            // TODO Open Inventory
            // Choose item
            StartCoroutine(_state.UseItem(null));
        }
        public void OnEscapeButton()
        {
            StartCoroutine(_state.Escape());
        }
    }
}
