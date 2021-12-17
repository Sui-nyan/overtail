using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Overtail.Battle
{

    public abstract class StateMachine : MonoBehaviour
    {
        protected State state;

        public void SetState(State state)
        {
            if (this.state != null)
            {
                StartCoroutine(this.state.Stop());
            }

            this.state = state;
            StartCoroutine(this.state.Start());
        }

        public void RestartState()
        {
            StartCoroutine(state.Start());
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

        private PlayerUnit _player;
        private EnemyUnit _enemy;

        [SerializeField] private BattleGUI _graphicalUI;
        public BattleGUI GUI => _graphicalUI;
        public PlayerUnit Player => _player;
        public EnemyUnit Enemy => _enemy;

        void Start()
        {
            UnityEngine.GameObject playerObject = Instantiate(battleSetupData.PlayerPrefab, playerStation);
            _player = playerObject.GetComponent<PlayerUnit>();

            UnityEngine.GameObject enemyGO = Instantiate(battleSetupData.EnemyPrefab, enemyStation);
            _enemy = enemyGO.GetComponent<EnemyUnit>();

            GUI.Setup(this);
            GUI.UpdateHUD();

            SetState(new StartState(this));
        }

        void FixedUpdate()
        {
            GUI.ReselectedGUI();
            GUI.UpdateHUD(); // performance pls?
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
            StartCoroutine(state.Attack());
        }
        public void OnInteractButton()
        {
            StartCoroutine(state.Interact());
        }
        public void OnInventoryButton()
        {
            StartCoroutine(state.Inventory());
        }
        public void OnEscapeButton()
        {
            StartCoroutine(state.Escape());
        }
    }
}