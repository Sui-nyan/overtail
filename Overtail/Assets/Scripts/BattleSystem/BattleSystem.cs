using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        [Header("Setup")]
        [SerializeField] private BattleSetupData _battleSetupData;
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private Transform _playerStation;
        [SerializeField] private Transform _enemyStation;

        [SerializeField] private PlayerUnit _player;
        [SerializeField] private EnemyUnit _enemy;

        [SerializeField] private BattleGUI _gui;
        public BattleGUI GUI => _gui;
        public PlayerUnit Player => _player;
        public EnemyUnit Enemy => _enemy;

        void Awake()
        {
            GameObject playerObject = Instantiate(_battleSetupData.PlayerPrefab, _playerStation);
            _player = playerObject.AddComponent<PlayerUnit>();
            _player.Load(_playerData);

            GameObject enemyGO = Instantiate(_battleSetupData.EnemyPrefab, _enemyStation);
            _enemy = enemyGO.GetComponent<EnemyUnit>();

            GUI.Setup(this);

            SetState(new StartState(this));
        }

        public void ExitBattle()
        {
            Player.Save(_playerData);
            StartCoroutine(UnloadCoroutine());
        }

        public void EscapeBattle()
        {
            // Anything different?
            ExitBattle();
        }

        private IEnumerator UnloadCoroutine()
        {
            // TODO Transition or whatever
            SceneManager.LoadScene("SampleScene");
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
            GUI.InteractionSubMenu(()=> StartCoroutine(_state.Flirt()),() => StartCoroutine(_state.Bully()));
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