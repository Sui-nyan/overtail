using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

namespace Overtail.Battle
{

    public abstract class StateMachine : MonoBehaviour
    {
        protected State state;
        protected State previous;
        [Header("Debug")]
        [SerializeField] private string _current;
        [SerializeField] private string _previous;


        public void SetState(State state)
        {
            if (this.state != null)
            {
                _previous = this.state.GetType().ToString();
                StartCoroutine(this.state.Stop());
            }
            
            this.state = state;
            _current = state.GetType().ToString();

            StartCoroutine(this.state.Start());
        }

        public void RestartState()
        {
            StartCoroutine(state.Start());
        }

        public void Lock()
        {
            previous = state;
            _previous = state.GetType().ToString();

            state = new LockState((BattleSystem)this);
            _current = state.GetType().ToString();
        }

        public void Unlock()
        {
            if (previous != null)
            {
                state = previous;
                _current = state.GetType().ToString();
            }
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
        public BattleSetupData setup;

        public BattleHUD playerHUD;
        public BattleHUD enemyHUD;

        public Transform playerStation;
        public Transform enemyStation;

        public BattleUnit playerUnit;
        public BattleUnit enemyUnit;

        public Text textBox;

        [SerializeField] private GameObject attackButton, interactButton, inventoryButton, escapeButton;
        [SerializeField] private GameObject lastPressedButton;

        GameObject[] Buttons => new GameObject[] { attackButton, interactButton, inventoryButton, escapeButton };

        void Start()
        {
            GameObject playerObject = Instantiate(setup.playerPrefab, playerStation);
            playerUnit = playerObject.GetComponent<BattleUnit>();

            GameObject enemyGO = Instantiate(setup.enemyPrefab, enemyStation);
            enemyUnit = enemyGO.GetComponent<BattleUnit>();

            UpdateHUD();
            lastPressedButton = attackButton;

            SetState(new StartState(this));
        }

        private void FixedUpdate()
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(lastPressedButton);
            }
        }

        public void UpdateHUD()
        {
            playerHUD.SetHUD(playerUnit);
            enemyHUD.SetHUD(enemyUnit);
        }



        public void Exit()
        {
            StartCoroutine(_unload());
        }

        private IEnumerator _unload()
        {

            SceneManager.LoadScene("SampleScene");
            yield break;
        }

        public void UpdatePlayerData()
        {

        }


        public void ShowButtons()
        {
            foreach (GameObject b in Buttons)
            {
                if (b == null) throw new System.Exception("Button is null - Buttons might not have been assigned in Unity");
                b.SetActive(true);
            }
        }

        public void HideButtons()
        {
            foreach (GameObject b in Buttons)
            {
                b.SetActive(false);
            }
        }

        public void OnAttackButton()
        {
            lastPressedButton = attackButton;
            StartCoroutine(state.Attack());
        }
        public void OnInteractButton()
        {
            lastPressedButton = interactButton;
            StartCoroutine(state.Interact());
        }
        public void OnInventoryButton()
        {
            lastPressedButton = inventoryButton;
            StartCoroutine(state.Inventory());
        }
        public void OnEscapeButton()
        {
            lastPressedButton = escapeButton;
            StartCoroutine(state.Escape());
        }
    }

    public class PlayerButtonPrefabs
    {
        public GameObject attackButton;
        public GameObject interactButton;
        public GameObject inventoryButton;
        public GameObject escapeButton;
    }
}