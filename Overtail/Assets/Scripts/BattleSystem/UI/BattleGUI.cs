using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Overtail.Battle
{
    [System.Serializable]
    public partial class BattleGUI : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] [NotNull] private BattleHUD _playerHud;
        [SerializeField] [NotNull] private BattleHUD _enemyHud;
        [SerializeField] [NotNull] private TextMeshProUGUI _tmpText;

        [Header("Config")]
        [SerializeField] private float _typeWriteDelay = 0.05f;
        [SerializeField] private float _guiDelayMin = 0.5f;
        [SerializeField] private float _guiDelayMax = 4f;

        [Header("Buttons")]
        [SerializeField] [NotNull] private GameObject _attackButton;
        [SerializeField] [NotNull] private GameObject _interactButton;
        [SerializeField] [NotNull] private GameObject _inventoryButton;
        [SerializeField] [NotNull] private GameObject _escapeButton;
        
        private Queue<GuiCoroutine> _guiEventQueue = new Queue<GuiCoroutine>();
        
        public bool IsIdle { get; private set; } = true;
        public bool GetConfirmKeyDown => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);
        private GameObject[] AllButtons => new GameObject[] { _attackButton, _interactButton, _inventoryButton, _escapeButton };

        public void Setup(BattleSystem system)
        {
            system.Player.StatusUpdate += UpdateHUD;
            system.Enemy.StatusUpdate += UpdateHUD;

            _playerHud.UpdateHUD(system.Player);
            _enemyHud.UpdateHUD(system.Enemy);

            foreach (GameObject b in AllButtons)
            {
                b.GetComponent<Button>().onClick.AddListener(HideButtons);
            }

            _attackButton.GetComponent<Button>().onClick.AddListener(system.OnAttackButton);
            _interactButton.GetComponent<Button>().onClick.AddListener(system.OnInteractButton);
            _inventoryButton.GetComponent<Button>().onClick.AddListener(system.OnInventoryButton);
            _escapeButton.GetComponent<Button>().onClick.AddListener(system.OnEscapeButton);

            HideButtons();
        }

        public void QueueMessage(string msg)
        {
            QueueMessage(msg, _guiDelayMin);
        }
        public void QueueCoroutine(Func<MonoBehaviour, IEnumerator> coroutineHandle)
        {
            QueueCoroutine(coroutineHandle, _guiDelayMin);
        }

        private void Update()
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(_attackButton);
            }
        }

        #region Coroutine Queue
        public void QueueMessage(string msg, float postEventDelay) // Wrap message as Coroutine
        {
            QueueMessage(msg, postEventDelay, _typeWriteDelay);
        }

        public void QueueMessage(string msg, float postEventDelay, float typeWriteDelay) // Wrap message as Coroutine
        {
            IEnumerator TextWrapper(MonoBehaviour obj) { return TypeWrite(msg, typeWriteDelay); }
            QueueCoroutine(TextWrapper, postEventDelay);
        }

        
        public void QueueCoroutine(Func<MonoBehaviour, IEnumerator> coroutine, float postEventDelay)
        {
            var s = new GuiCoroutine();
            s.CoroutineHandle = coroutine;
            s.PostEventDelay = postEventDelay;
            _guiEventQueue.Enqueue(s);

            if (IsIdle)
            {
                IsIdle = false;
                StartCoroutine(ProcessQueue());
            }
        }

        private IEnumerator ProcessQueue()
        {
            while (_guiEventQueue.Count > 0)
            {
                var next = _guiEventQueue.Dequeue() as GuiCoroutine;
                Debug.Log($"NextEvent:[{next.PostEventDelay:.00}s][{next.CoroutineHandle.Method.Name}]");
                yield return StartCoroutine(next.CoroutineHandle(this));
                yield return AwaitTimeOrConfirm(minDuration: next.PostEventDelay, _guiDelayMax);
                yield return new WaitForEndOfFrame();
            }

            IsIdle = true;
        }

        public Coroutine AwaitIdle()
        {
            IEnumerator Wait() { yield return new WaitUntil(() => IsIdle); }
            return StartCoroutine(Wait());
        }

        public Coroutine AwaitTimeOrConfirm()
        {
            return AwaitTimeOrConfirm(_guiDelayMin, _guiDelayMax);
        }
        public Coroutine AwaitTimeOrConfirm(float minDuration, float maxDuration)
        {
            IEnumerator WaitOrConfirm(float min, float max)
            {
                max -= min;
                yield return new WaitForSeconds(min);
                yield return new WaitUntil(() =>
                {
                    max -= Time.deltaTime;
                    return max < 0 || GetConfirmKeyDown;
                });
                yield return new WaitForEndOfFrame();
            }

            maxDuration = Math.Max(minDuration, maxDuration);
            return StartCoroutine(WaitOrConfirm(minDuration, maxDuration));
        }

        private IEnumerator TypeWrite(string text, float typingDelay)
        {
            _tmpText.text = string.Empty;

            var timeElapsed = 0f;
            int i = 0;

            while (i < text.Length)
            {
                if (GetConfirmKeyDown)
                {
                    _tmpText.text = text;
                    break;
                }

                if (timeElapsed > typingDelay)
                {
                    timeElapsed = 0;
                    i++;
                    _tmpText.text = text.Substring(0, i);
                    _tmpText.text += "<color=#00000000>" + text.Substring(i) + "</color>";
                }

                timeElapsed += Time.deltaTime; // time since last frame
                yield return null; // Skip to next frame
            }
            yield return new WaitForSeconds(0f);
        }

        #endregion

        #region GUI Interface

        public void UpdateHUD(BattleUnit obj)
        {
            var type = obj.GetType();

            if (type == typeof(PlayerUnit))
                _playerHud.UpdateHUD(obj);//_UpdateHud(obj, playerHUD);
            else if (type.IsSubclassOf(typeof(EnemyUnit)))
                _enemyHud.UpdateHUD(obj);//_UpdateHud(obj, enemyHUD);
            else
                throw new ArgumentException($"Invalid type {obj.GetType().Name}:{typeof(BattleUnit).Name} ");
        }

        public void ShowButtons()
        {
            foreach (GameObject b in AllButtons)
            {
                if (b == null)
                    throw new System.Exception("Button is null - Buttons might not have been assigned in Unity");
                b.SetActive(true);
            }
        }

        public void HideButtons()
        {
            foreach (GameObject b in AllButtons)
            {
                b.SetActive(false);
            }
            AwaitTimeOrConfirm();
        }

        public void InteractionSubMenu(Func<UnityEngine.Coroutine> flirtFunc, Func<UnityEngine.Coroutine> bullyFunc)
        {
            // TODO Proper layout and code structure
            GameObject[] buttons = new GameObject[2];
            GameObject CreateButton(string label, Func<UnityEngine.Coroutine> func, Vector2 pos)
            {
                var root = GameObject.Find("DialogueBox");
                var prefab = Resources.Load<GameObject>("_Button");
                var buttonGameObject = GameObject.Instantiate(prefab, root.transform);
                var button = buttonGameObject.GetComponent<Button>();

                var t = buttonGameObject.GetComponentInChildren<Text>();
                t.text = label;
                t.fontSize = 64;

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
                ((RectTransform) buttonGameObject.transform).sizeDelta = new Vector2(200, 100);


                return buttonGameObject;
            }

            buttons[0] = CreateButton("Flirt", flirtFunc, new Vector2(200, 0));
            buttons[1] = CreateButton("Bully", bullyFunc, new Vector2(600, 0));
        }

        #endregion
    }
}