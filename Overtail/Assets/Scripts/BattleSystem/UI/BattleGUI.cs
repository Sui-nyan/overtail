using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Overtail.Battle
{
    [System.Serializable]
    public class BattleGUI
    {
        [Header("UI Elements")]
        [SerializeField] private BattleHUD playerHUD;
        [SerializeField] private BattleHUD enemyHUD;
        [SerializeField] private Text textBox;

        [Header("Buttons")]
        [SerializeField] private GameObject attackButton;
        [SerializeField] private GameObject interactButton;
        [SerializeField] private GameObject inventoryButton;
        [SerializeField] private GameObject escapeButton;

        private BattleSystem _system;
        private BattleUnit _player;
        private BattleUnit _enemy;

        private Queue<string> _messageQueue;
        private bool _isBusy;
        private GameObject[] Buttons => new GameObject[] {
            attackButton,
            interactButton,
            inventoryButton,
            escapeButton
        };

        public void Setup(BattleSystem system)
        {
            _player.ObjectSpeaking += QueueMessage;
            _enemy.ObjectSpeaking += QueueMessage;

            _player.Updated += UpdateHUD;
            _enemy.Updated += UpdateHUD;

            this._system = system;
            this._player = this._system.Player;
            this._enemy = this._system.Enemy;

            foreach (GameObject b in Buttons)
            {
                b.GetComponent<Button>().onClick.AddListener(HideButtons);
            }
        }

        private void UpdateHUD(BattleUnit obj)
        {
            // If lost HP
            //  Do slow HP drop animation
            throw new System.NotImplementedException();
        }

        private void QueueMessage(string msg)
        {
            // Queue message
            _messageQueue.Enqueue(msg);
            _system.StartCoroutine(ProcessQueue());
        }

        private IEnumerator ProcessQueue()
        {
            if (_isBusy) yield break;

            _isBusy = true;

            while (_messageQueue.Count > 0)
            {
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            }

            _isBusy = false;
        }

        



        public void UpdateHUD()
        {
            playerHUD.SetHUD(_player);
            enemyHUD.SetHUD(_enemy);
        }

        public void SetText(string text)
        {
            textBox.text = text;
        }

        public void ReselectedGUI()
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(attackButton);
            }
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
    }
}