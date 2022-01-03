using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Overtail.Battle
{

    public abstract class StateMachine : MonoBehaviour
    {
        protected State _state;

        public void SetState(State state)
        {
            if (this._state != null)
            {
                StartCoroutine(this._state.CleanUp());
            }

            this._state = state;
            StartCoroutine(this._state.Start());
        }

        public void RestartState()
        {
            StartCoroutine(_state.Start());
        }
    }

    /// <summary>
    /// Overarching system for Battle Scenes.
    /// Logical main entry point. Sets up the battle, starts it and manages the state (State machine).
    /// Add component to some gameObject
    /// Credits to Brackeys - https://www.youtube.com/watch?v=_1pz_ohupPs 
    /// </summary>
    public class BattleSystem : StateMachine
    {
        [Header("Setup")]
        [SerializeField] private BattleSetupData battleSetupData;
        [SerializeField] private Transform playerStation;
        [SerializeField] private Transform enemyStation;

        [SerializeField] private PlayerUnit _player;
        [SerializeField] private EnemyUnit _enemy;

        [SerializeField] private BattleGUI _graphicalUI;
        public BattleGUI GUI => _graphicalUI;
        public PlayerUnit Player => _player;
        public EnemyUnit Enemy => _enemy;

        public bool IsIdle => !GUI.IsBusy;

        void Start()
        {
            GameObject playerObject = Instantiate(battleSetupData.PlayerPrefab, playerStation);
            _player = playerObject.GetComponent<PlayerUnit>();

            GameObject enemyGO = Instantiate(battleSetupData.EnemyPrefab, enemyStation);
            _enemy = enemyGO.GetComponent<EnemyUnit>();

            GUI.Setup(this);
            GUI.UpdateHud();

            SetState(new StartState(this));
        }

        void FixedUpdate()
        {
            GUI.ReselectGui();
        }

        public void Exit()
        {
            StartCoroutine(Unload());
        }

        private IEnumerator Unload()
        {
            Player.End();
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
            GUI.FlirtOrBully(()=> StartCoroutine(_state.Flirt()),() => StartCoroutine(_state.Bully()));
        }
        public void OnInventoryButton()
        {
            // Open Inventory
            // Choose item
            StartCoroutine(_state.UseItem(null));
        }
        public void OnEscapeButton()
        {
            StartCoroutine(_state.Escape());
        }
    }
}