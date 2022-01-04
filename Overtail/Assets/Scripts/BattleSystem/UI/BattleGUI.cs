using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Overtail.Battle
{
    [System.Serializable]
    public class BattleGUI
    {
        [Header("UI Elements")] [SerializeField]
        private BattleHUD playerHUD;

        [SerializeField] private BattleHUD enemyHUD;
        [SerializeField] private Text textBox;

        [Header("Buttons")] [SerializeField] private GameObject attackButton;
        [SerializeField] private GameObject interactButton;
        [SerializeField] private GameObject inventoryButton;
        [SerializeField] private GameObject escapeButton;

        private BattleSystem _system;
        private BattleUnit _player;
        private BattleUnit _enemy;

        // TODO make this an action/GUIUpdate queue
        private Queue<string> _messageQueue = new Queue<string>();
        private KeyCode _confirmKey = KeyCode.Space;
        private bool _isBusy = false;
        public bool IsBusy => _isBusy;
        private GameObject[] Buttons => new GameObject[]
        {
            attackButton,
            interactButton,
            inventoryButton,
            escapeButton
        };

        public void Setup(BattleSystem system)
        {
            _system = system;

            _player = _system.Player;
            _enemy = _system.Enemy;

            foreach (GameObject b in Buttons)
            {
                b.GetComponent<Button>().onClick.AddListener(HideButtons);
            }

            _player.ObjectSpeaking += QueueMessage;
            _enemy.ObjectSpeaking += QueueMessage;

            _player.Updated += UpdateHud;
            _enemy.Updated += UpdateHud;
        }

        private void UpdateHud(BattleUnit obj)
        {
            // TODO Check for what changed

            // E.g. slowly drop HP
        }

        public void QueueMessage(string msg)
        {
            // Queue message
            _messageQueue.Enqueue(msg);

            if (_isBusy) return;

            _isBusy = true;
            _system.StartCoroutine(ProcessQueue());
        }

        private IEnumerator ProcessQueue()
        {
            while (_messageQueue.Count > 0)
            {
                var text = _messageQueue.Dequeue();
                yield return TypeWriteText(text);
                yield return AwaitTimeOrConfirm(minWait : 0.5f);
                yield return new WaitForEndOfFrame();
            }

            _isBusy = false;
        }


        public void UpdateHud()
        {
            playerHUD.SetHUD(_player);
            enemyHUD.SetHUD(_enemy);
        }

        public void SetText(string text)
        {
            textBox.text = text;
        }

        public void ReselectGui()
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
                if (b == null)
                    throw new System.Exception("Button is null - Buttons might not have been assigned in Unity");
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

        public Coroutine AwaitTimeOrConfirm(float maxWait = 4f, float minWait = 0.2f)
        {
            IEnumerator _WaitOrConfirm(float maxTime = 4f, float minTime = 0.2f)
            {
                maxTime -= minTime;
                yield return new WaitForSeconds(minTime);
                yield return new WaitUntil(() =>
                {
                    maxTime -= Time.deltaTime;
                    return maxTime < 0 || Input.GetKey(_confirmKey);
                });
                yield return new WaitForEndOfFrame();
            }
            return _system.StartCoroutine(_WaitOrConfirm(maxWait, minWait));
        }

        private Coroutine TypeWriteText(string text, float delay = 0.09f)
        {
            Debug.Log($"{text}");

            IEnumerator Typing()
            {
                textBox.text = string.Empty;
                var timeElapsed = 0f;
                int i = 0;
                while(i < text.Length)
                {
                    if (Input.GetKeyDown(_confirmKey))
                    {
                        textBox.text = text;
                        break;
                    }

                    if (timeElapsed > delay)
                    {
                        timeElapsed = 0;
                        textBox.text = text.Substring(0, i);
                        i++;
                    }

                    timeElapsed += Time.deltaTime; // time since last frame
                    yield return null; // Skip to next frame
                }
                yield return new WaitForSeconds(0f);
            }
            return _system.StartCoroutine(Typing());
        }



        public void InteractionSubMenu(Func<Coroutine> flirtFunc, Func<Coroutine> bullyFunc)
        {
            GameObject[] buttons = new GameObject[2];
            GameObject CreateButton(string label, Func<Coroutine> func, Vector2 pos)
            {
                var root = GameObject.Find("DialogueBox");
                var prefab = Resources.Load<GameObject>("_Button");
                var buttonGameObject = GameObject.Instantiate(prefab, root.transform);
                var button = buttonGameObject.GetComponent<Button>();

                buttonGameObject.GetComponentInChildren<Text>().text = label;

                button.onClick.AddListener(() =>
                {
                    foreach (var b in buttons)
                    {
                        GameObject.Destroy(b);
                    }
                    func();
                });
                
                buttonGameObject.name = label;
                buttonGameObject.transform.localPosition = new Vector3(pos.x, pos.y, 0);

                return buttonGameObject;
            }

            buttons[0] = CreateButton("Flirt", flirtFunc, new Vector2(160, 40));
            buttons[1] = CreateButton("Bully", bullyFunc, new Vector2(160, 0));
        }
    }
}