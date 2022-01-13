using System;
using System.Collections;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Overtail.GUI
{
    [DisallowMultipleComponent]
    public class TabGroup : MonoBehaviour
    {
        public float indicatorOffset = 20f;

        [Header("Initialize")] [SerializeField]
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
            SetOnClicks();
        }

        private void SetOnClicks()
        {
            for (int i = 0; i < _tabs.Length; i++)
            {
                var index = i;
                var button = _tabs[i].GetComponent<Button>();
                AddOnSelectAction(button.gameObject, () => { SetTab(index); });
            }
        }

        private void AddOnSelectAction(GameObject selectableObj, Action f)
        {
            if (!selectableObj.TryGetComponent<EventTrigger>(out var trigger))
                trigger = selectableObj.AddComponent<EventTrigger>();

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Select;
            entry.callback.AddListener((eventData) => f());

            trigger.triggers.Add(entry);
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
            _panelGroup.SetPanel(_tabIndex);
            // Debug.Log($"[Tab] Clicked {_tabIndex}:{_tabs[_tabIndex].name}");

            SetIndicator(_tabIndex);
        }

        public void OnOpen()
        {
        }

        public void OnExit()
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

        public void SetTab(string tabName)
        {
            var index = GetTabIndex(tabName);
            SetTab(index >= 0 ? index : 0);
        }
    }
}