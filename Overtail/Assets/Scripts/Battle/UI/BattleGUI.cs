using System;
using System.Collections;
using Overtail.Battle.Entity;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Overtail.Battle.UI
{
    public partial class BattleGUI : MonoBehaviour
    {
    }


    [System.Serializable]
    public partial class BattleGUI : MonoBehaviour
    {
        [Header("UI Elements")] [SerializeField] [NotNull] private BattleHUD _playerHud;
        [SerializeField] [NotNull] private BattleHUD _enemyHud;
        [SerializeField] [NotNull] private TextMeshProUGUI _tmpText;

        [Header("Config")] [SerializeField] private float _typeWriteDelay = 0.05f;
        [SerializeField] private float _delayMin = 0.5f;
        [SerializeField] private float _delayMax = 4f;

        [Header("Buttons")] [SerializeField] [NotNull] private GameObject _attackButton;
        [SerializeField] [NotNull] private GameObject _interactButton;
        [SerializeField] [NotNull] private GameObject _inventoryButton;
        [SerializeField] [NotNull] private GameObject _escapeButton;

        public bool IsIdle { get; private set; } = true;
        public bool GetConfirmKeyDown => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);

        private GameObject[] AllButtons => new GameObject[]
            {_attackButton, _interactButton, _inventoryButton, _escapeButton};

        public void Setup(BattleSystem system)
        {
            system.Player.StatusUpdated += UpdateHUD;
            system.Enemy.StatusUpdated += UpdateHUD;

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

        private void Update()
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(_attackButton);
            }
        }

        #region Coroutines

        public Coroutine StartDialogue(string text)
        {
            return StartDialogue(text, typeWriteDelay: _typeWriteDelay, delayMin: _delayMin, delayMax: _delayMax);
        }

        /// <summary>
        /// Uses defaults from Unity Inspector if not set
        /// </summary>
        /// <param name="text"></param>
        /// <param name="typeWriteDelay"></param>
        /// <param name="delayMin"></param>
        /// <param name="delayMax"></param>
        /// <returns></returns>
        public Coroutine StartDialogue(string text, float typeWriteDelay = -1f, float delayMin = -1f,
            float delayMax = -1f, bool skipAvailable = true)
        {
            if (typeWriteDelay < 0) typeWriteDelay = _typeWriteDelay;
            if (delayMin < 0) delayMin = _delayMin;
            if (delayMax < 0) delayMax = _delayMax;

            return StartCoroutine(Dialogue(
                text: text,
                typingDelay: typeWriteDelay,
                minDelay: delayMin,
                maxDelay: delayMax,
                skipAvailable:skipAvailable));
        }

        public IEnumerator Dialogue(string text, float typingDelay, float minDelay, float maxDelay, bool skipAvailable)
        {
            yield return StartCoroutine(TypeWriteText(text, typingDelay));
            yield return AwaitTimeOrConfirm(minDelay, maxDelay);
            yield return null;

            IEnumerator TypeWriteText(string text, float typingDelay)
            {
                _tmpText.text = string.Empty;

                var timeElapsed = 0f;
                int i = 0;

                while (i < text.Length)
                {
                    if (skipAvailable && GetConfirmKeyDown)
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
            }
        }

        public Coroutine AwaitIdle()
        {
            IEnumerator Wait()
            {
                yield return new WaitUntil(() => IsIdle);
            }

            return StartCoroutine(Wait());
        }

        public Coroutine AwaitTimeOrConfirm()
        {
            return AwaitTimeOrConfirm(_delayMin, _delayMax);
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

        #endregion

        #region GUI Interface

        public void UpdateHUD(BattleEntity obj)
        {
            var type = obj.GetType();

            if (type == typeof(PlayerEntity))
                _playerHud.UpdateHUD(obj); //_UpdateHud(obj, playerHUD);
            else if (type.IsSubclassOf(typeof(EnemyEntity)))
                _enemyHud.UpdateHUD(obj); //_UpdateHud(obj, enemyHUD);
            else
                throw new ArgumentException($"Invalid type {obj.GetType().Name}:{typeof(BattleEntity).Name} ");
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

        public Coroutine SmoothExpBar(float progress)
        {
            return StartCoroutine(_playerHud.SmoothExp(progress));
        }
    }
}
