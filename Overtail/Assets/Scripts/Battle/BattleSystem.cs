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
        [SerializeField] private BattleSetup _setup;
        [SerializeField] private BattleResult _result;

        [SerializeField] private Transform _playerStation;
        [SerializeField] private Transform _enemyStation;

        [SerializeField] private PlayerEntity _playerEntity;
        [SerializeField] private EnemyEntity _enemyEntity;

        private BattleGUI _gui;
        public BattleGUI GUI => _gui;
        public PlayerEntity Player => _playerEntity;
        public EnemyEntity Enemy => _enemyEntity;

        void Awake()
        {
            _gui = FindObjectOfType<BattleGUI>();
            if (_gui is null) Debug.LogError("Failed to initialize GUI");

            var playerPrefab = Resources.Load<PlayerEntity>("Prefabs/PlayerEntity")?.gameObject;
            if (playerPrefab is null) Debug.LogError($"No player prefab found{playerPrefab}");

            _playerEntity = Instantiate(playerPrefab, _playerStation).GetComponent<PlayerEntity>();

            GameObject enemyGO = Instantiate(_setup.enemy.gameObject, _enemyStation);
            _enemyEntity = enemyGO.GetComponent<EnemyEntity>();
        }

        void Start()
        {
            var p = FindObjectOfType<Player>();
            //_playerEntity.Load(_setup.player);
            _playerEntity.Load(p);

            GUI.Setup(this);
            SetState(new StartState(this));
        }

        public void Exit()
        {
            var p = FindObjectOfType<Player>();
            //_playerEntity.Save(_result);
            _playerEntity.Save(p);
            // Destroy(_setup.player.gameObject);
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
