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
        protected State _state;
        public void SetState(State state)
        {
            if (_state != null) StartCoroutine(_state.Stop());
            _state = state;
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
            StartCoroutine(_state.Attack());
        }
        public void OnInteractButton()
        {
            lastPressedButton = interactButton;
            StartCoroutine(_state.Interact());
        }
        public void OnInventoryButton()
        {
            lastPressedButton = inventoryButton;
            StartCoroutine(_state.Inventory());
        }
        public void OnEscapeButton()
        {
            lastPressedButton = escapeButton;
            StartCoroutine(_state.Escape());
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