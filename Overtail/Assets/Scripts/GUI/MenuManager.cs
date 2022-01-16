using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using Overtail.Util;

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

        private bool _isAssigned => _mainMenu != null && _panelGroup != null && _tabGroup != null;

        private void Awake()
        {
            MonoBehaviourExtension.MakeSingleton(this, ref _instance, keepAlive: true, destroyOnSceneZero: true);

            TryAssign();
        }

        private void Start()
        {
            InputManager.Instance.KeyMenu += ToggleMenu;
            InputManager.Instance.KeyInventory += () => ToggleMenu("Inventory");
            InputManager.Instance.KeyOptions += () => ToggleMenu("Settings");

            SceneLoader.Instance.SceneChanged += TryAssign;
        }

        private void TryAssign()
        {
            var canvas = FindObjectOfType<Canvas>();
            if (canvas == null) return;

            TryGetFirst<PanelGroup>(canvas.gameObject, ref _panelGroup);
            if (_panelGroup == null) return;

            _mainMenu = _panelGroup.transform.parent.gameObject;
            TryGetFirst<TabGroup>(_mainMenu, ref _tabGroup);

            StartCoroutine(QuickWakeUp(_mainMenu));
        }

        private bool TryGetFirst<T>([NotNull] GameObject parent, ref T obj) where T : MonoBehaviour
        {
            if (parent == null) throw new ArgumentNullException(nameof(parent));

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

        private IEnumerator QuickWakeUp(GameObject o)
        {
            o.SetActive(true);
            yield return null;
            o.SetActive(false);
        }


        private void ToggleMenu()
        {
            if (!_isAssigned) return;

            if (_mainMenu.activeSelf)
            {
                OnClose();
            }
            else
            {
                OnOpen();
            }
        }

        private void ToggleMenu(string tabName)
        {
            if (!_isAssigned) return;

            var index = _tabGroup.GetTabIndex(tabName);
            if (index < 0)
            {
                throw new ArgumentException("Unknown TabName");
            }

            if (_mainMenu.activeSelf)
            {
                if (_tabGroup.TabIndex != index)
                {
                    _tabGroup.SetTab(index);
                }
                else
                {
                    OnClose();
                }
            }
            else
            {
                OnOpen();
                _tabGroup.SetTab(index);
            }
        }

        public void OnOpen()
        {
            _mainMenu.SetActive(true);

            _panelGroup.OnOpen();
            _tabGroup.OnOpen();

            MenuOpened?.Invoke();
        }

        public void OnClose()
        {
            _panelGroup.OnClose();
            _tabGroup.OnExit();

            _mainMenu.SetActive(false);

            MenuClosed?.Invoke();
        }
    }
}