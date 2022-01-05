using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Overtail.Battle
{
    [System.Serializable]
    public partial class BattleGUI : MonoBehaviour
    {
        [Header("UI Elements")]

        [SerializeField] private BattleHUD playerHUD;
        [SerializeField] private BattleHUD enemyHUD;
        [SerializeField] private Text textBox;

        [Header("Buttons")] [SerializeField] private GameObject attackButton;
        [SerializeField] private GameObject interactButton;
        [SerializeField] private GameObject inventoryButton;
        [SerializeField] private GameObject escapeButton;

        private BattleSystem _system;
        private BattleUnit _player;
        private BattleUnit _enemy;

        private Queue<GuiCoroutine> _guiEventQueue = new Queue<GuiCoroutine>();

        public float GuiDelayMin { get; set; } = 0.5f;
        public float GuiDelayMax { get; set; } = 4f;

        public float TypeWriteDelay = 0.05f;
        private bool GetConfirmationDown => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);
        public bool IsIdle { get; private set; } = true;
        private GameObject[] Buttons => new GameObject[] { attackButton, interactButton, inventoryButton, escapeButton };

        public void Setup(BattleSystem system)
        {
            _system = system;

            _player = _system.Player;
            _enemy = _system.Enemy;

            _player.StatusUpdate += UpdateHUD;
            _enemy.StatusUpdate += UpdateHUD;

            playerHUD.UpdateHUD(_player);
            enemyHUD.UpdateHUD(_enemy);

            foreach (GameObject b in Buttons)
            {
                b.GetComponent<Button>().onClick.AddListener(HideButtons);
            }
        }


        public void QueueMessage(string msg)
        {
            QueueMessage(msg, GuiDelayMin);
        }
        public void QueueCoroutine(Func<MonoBehaviour, IEnumerator> coroutineHandle)
        {
            QueueCoroutine(coroutineHandle, GuiDelayMin);
        }

        private void Update()
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(attackButton);
            }
        }

        #region Coroutine Queue
        public void QueueMessage(string msg, float postEventDelay) // Wrap message as Coroutine
        {
            QueueMessage(msg, postEventDelay, TypeWriteDelay);
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
                yield return AwaitTimeOrConfirm(minDuration: next.PostEventDelay, GuiDelayMax);
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
            return AwaitTimeOrConfirm(GuiDelayMin, GuiDelayMax);
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
                    return max < 0 || GetConfirmationDown;
                });
                yield return new WaitForEndOfFrame();
            }

            maxDuration = Math.Max(minDuration, maxDuration);
            return StartCoroutine(WaitOrConfirm(minDuration, maxDuration));
        }

        private IEnumerator TypeWrite(string text, float typingDelay)
        {
            textBox.text = string.Empty;
            var timeElapsed = 0f;
            int i = 0;
            while (i < text.Length)
            {
                if (GetConfirmationDown)
                {
                    textBox.text = text;
                    break;
                }

                if (timeElapsed > typingDelay)
                {
                    timeElapsed = 0;
                    i++;
                    textBox.text = text.Substring(0, i);
                }

                timeElapsed += Time.deltaTime; // time since last frame
                yield return null; // Skip to next frame
            }
            yield return new WaitForSeconds(0f);
        }

        #endregion

        #region GUI Interface
        public void SetHUDs()
        {
            playerHUD.UpdateHUD(_player);
            enemyHUD.UpdateHUD(_enemy);
        }
        public void UpdateHUD(BattleUnit obj)
        {
            var type = obj.GetType();

            if (type == typeof(PlayerUnit))
                playerHUD.UpdateHUD(obj);//_UpdateHud(obj, playerHUD);
            else if (type.IsSubclassOf(typeof(EnemyUnit)))
                enemyHUD.UpdateHUD(obj);//_UpdateHud(obj, enemyHUD);
            else
                throw new ArgumentException($"Invalid type {obj.GetType().Name}:{typeof(BattleUnit).Name} ");
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
            AwaitTimeOrConfirm();
        }

        public void InteractionSubMenu(Func<UnityEngine.Coroutine> flirtFunc, Func<UnityEngine.Coroutine> bullyFunc)
        {
            GameObject[] buttons = new GameObject[2];
            GameObject CreateButton(string label, Func<UnityEngine.Coroutine> func, Vector2 pos)
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

        #endregion
    }
}