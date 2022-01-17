using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Overtail.GUI
{
    [DisallowMultipleComponent]
    public class TabGroup : MonoBehaviour
    {
        public float indicatorOffset = 20f;

        [Header("Initialize")]
        [SerializeField]
        private GameObject indicator;

        [SerializeField] private PanelGroup _panelGroup;

        [SerializeField] private GameObject[] _tabs;


        private int _tabIndex;

        public int TabIndex => _tabIndex;

        private void Awake()
        {
            //_panelGroup = UnityHelper.FindSingleObjectOfType<PanelGroup>();
            _tabs = GetComponentsInChildren<Tab>().Select(t => t.gameObject).ToArray();
        }

        private void Start()
        {
            SetNavigation();
            SetOnSelection();
            SetOnClick();
        }

        private void SetOnClick()
        {
            for (int i = 0; i < _tabs.Length; i++)
            {
                var index = i;
                var b = _tabs[i].GetComponent<Button>();
                b.onClick.AddListener(EnterPanel);
            }
        }

        private void SetNavigation()
        {
            var total = _tabs.Length;
            for (int i = 0; i < _tabs.Length; i++)
            {
                var index = i;
                var button = _tabs[i].GetComponent<Button>();
                var nav = button.navigation;
                nav.mode = Navigation.Mode.Explicit;
                nav.selectOnDown = _tabs[(i + 1) % total].GetComponent<Button>();
                nav.selectOnUp = _tabs[(total + i - 1) % total].GetComponent<Button>();
                button.navigation = nav;

                // Add Event Trigger to right navigation
                if (!button.TryGetComponent<EventTrigger>(out var trigger))
                    trigger = button.gameObject.AddComponent<EventTrigger>();

                EventTrigger.Entry entry = new EventTrigger.Entry();

                entry.eventID = EventTriggerType.Move;
                entry.callback.AddListener((eventData) =>
                {
                    var e = (AxisEventData)eventData;
                    if (e.moveDir == MoveDirection.Right) EnterPanel();
                });

                trigger.triggers.Add(entry);
            }
        }

        private void SetOnSelection()
        {
            for (int i = 0; i < _tabs.Length; i++)
            {
                var index = i;
                var button = _tabs[i].GetComponent<Button>();

                // Add Event Trigger to Selection
                if (!button.TryGetComponent<EventTrigger>(out var trigger))
                    trigger = button.gameObject.AddComponent<EventTrigger>();

                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.Select;
                entry.callback.AddListener((eventData) => SetTab(index));

                trigger.triggers.Add(entry);
            }
        }

        private void SetIndicator(int index)
        {
            //var indicatorOffset = 40;

            var rect = _tabs[index].GetComponent<RectTransform>();
            // no null check because shouldn't be null

            Vector3[] v = new Vector3[4];
            rect.GetLocalCorners(v);
            var leftBottom = v[0];

            var pos = rect.localPosition;
            pos.x = leftBottom.x - indicatorOffset;

            indicator.GetComponent<RectTransform>().localPosition = pos;
        }

        public void SetTab(int index)
        {
            _tabIndex = index;
            SetIndicator(_tabIndex);
            _panelGroup.SetPanel(_tabIndex);
        }

        public void EnterUI()
        {
            EventSystem.current.SetSelectedGameObject(_tabs[_tabIndex]);
        }

        public void ExitUI()
        {

        }

        public int GetTabIndex(string tabName)
        {
            if (tabName == null) return -1;

            foreach (Transform child in transform)
            {
                if (child.gameObject.name.Contains(tabName))
                {
                    var index = _tabs.ToList().FindIndex(t => t == child.gameObject);
                    return index;
                }
            }

            return -1;
        }

        private void EnterPanel()
        {
            this.ExitUI();
            _panelGroup.EnterUI();
        }
    }
}
