using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using Overtail.Util;
using UnityEngine.EventSystems;

namespace Overtail.GUI
{
    public class MenuManager : MonoBehaviour
    {
        private static MenuManager _instance;
        public static MenuManager Instance => _instance;

        private GameObject _mainMenu;
        private PanelGroup _panelGroup;
        private TabGroup _tabGroup;

        public event Action MenuOpened;
        public event Action MenuClosed;

        private GameObject _lastSelection;
        public bool MenuIsActive => _mainMenu?.activeSelf ?? false;

        private void Awake()
        {
            MonoBehaviourExtension.MakeSingleton(this, ref _instance, keepAlive: true, destroyOnSceneZero: true);

            TryAssign();
        }

        private void Start()
        {
            InputManager.Instance.KeyMenu += OnMenuKey;
            InputManager.Instance.KeyInventory += () => OnSpecialMenuKey("Inventory");
            InputManager.Instance.KeyOptions += () => OnSpecialMenuKey("Settings");

            if (SceneLoader.Instance is null)
            {
                Debug.LogError("<color=red>SceneLoader not found</color>");
                throw new InvalidOperationException();
            }
            else
            {
                SceneLoader.Instance.SceneChanged += TryAssign;
            }

            //_tabGroup.EnterUI();
        }

        private void TryAssign()
        {
            _panelGroup = null;
            _tabGroup = null;
            _mainMenu = null;

            var canvas = FindObjectOfType<Canvas>();
            if (canvas == null) return;

            TryGetFirst<PanelGroup>(canvas.gameObject, ref _panelGroup);
            _mainMenu = _panelGroup?.transform.parent.gameObject;
            TryGetFirst<TabGroup>(_mainMenu, ref _tabGroup);

            if (_mainMenu == null || _panelGroup == null || _tabGroup == null)
            {
                Debug.LogWarning("Could not find MainMenu canvas");
                return;
            }

            StartCoroutine(QuickWakeUp(_mainMenu));

            IEnumerator QuickWakeUp(GameObject o)
            {
                o?.SetActive(true);
                yield return null;
                o?.SetActive(false);
            }
        }

        private bool TryGetFirst<T>(GameObject parent, ref T obj) where T : MonoBehaviour
        {
            if (parent == null) return false;//throw new ArgumentNullException(nameof(parent));

            foreach (Transform t in parent.transform)
            {
                if (t.TryGetComponent<T>(out var component))
                {
                    obj = component;
                    return true;
                }

                if (TryGetFirst<T>(t.gameObject, ref obj)) return true;
            }

            return false;
        }

        void Update()
        {
            if (_mainMenu == null || !_mainMenu.activeSelf) return;

            var sel = EventSystem.current?.currentSelectedGameObject;
            if (sel == null)
            {
                Debug.LogWarning("UI Navigation jumped out of context :: Refocus");
                EventSystem.current?.SetSelectedGameObject(_lastSelection);
            }

            if (GameObjectTree.ContainsGameObject(_panelGroup.gameObject, sel))
            {
                _lastSelection = sel;
            }

            if (GameObjectTree.ContainsGameObject(_tabGroup.gameObject, sel))
            {
                _lastSelection = sel;
            }
        }

        private void OnMenuKey()
        {
            if (_mainMenu.activeSelf)
            {
                var obj = EventSystem.current.currentSelectedGameObject;
                if (GameObjectTree.ContainsGameObject(_panelGroup.gameObject, obj))
                {
                    // If in panel group -> return to tabs
                    _panelGroup.ExitUI();
                    _tabGroup.EnterUI();
                }
                else
                {
                    // In tabs -> exit
                    CloseMenu();
                }
            }
            else
            {
                OpenMenu();
            }
        }

        private void OnSpecialMenuKey(string tabName)
        {

            var index = _tabGroup.GetTabIndex(tabName);
            if (index < 0)
            {
                throw new ArgumentException("Unknown TabName");
            }

            if (_mainMenu.activeSelf)
            {
                if (_tabGroup.TabIndex != index)
                {
                    _tabGroup.EnterUI();
                    _tabGroup.SetTab(index);
                    _tabGroup.ExitUI();

                    _panelGroup.EnterUI();
                }
                else // Current Tab
                {
                    CloseMenu();
                }
            }
            else
            {
                OpenMenu();

                _tabGroup.EnterUI();
                _tabGroup.SetTab(index);
                _tabGroup.ExitUI();

                _panelGroup.EnterUI();
            }
        }

        public void OpenMenu()
        {
            _mainMenu.SetActive(true);
            MenuOpened?.Invoke();
            //EventSystem.current.SetSelectedGameObject(_lastSelection);
        }

        public void CloseMenu()
        {
            _mainMenu.SetActive(false);
            MenuClosed?.Invoke();
        }
    }
}