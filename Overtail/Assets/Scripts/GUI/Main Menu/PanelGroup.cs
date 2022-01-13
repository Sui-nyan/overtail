using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace Overtail.GUI
{
    [DisallowMultipleComponent]
    public class PanelGroup : MonoBehaviour
    {
        [SerializeField] private GameObject[] _panels;

        [Header("Assign")] [SerializeField] private TabGroup _tabGroup;
        private int _panelIndex;

        public void Awake()
        {
            // _tabGroup = UnityHelper.FindSingleObjectOfType<TabGroup>();
            _panels = GetComponentsInChildren<Panel>(true).Select(c => c.gameObject).ToArray();
        }

        public void OnOpen()
        {
        }

        public void OnClose()
        {
        }

        private void ShowCurrentPanel()
        {
            for (int i = 0; i < _panels.Length; i++)
            {
                if (i == _panelIndex)
                {
                    _panels[i].SetActive(true);
                    _panels[i].GetComponent<Panel>()?.Refresh();
                }
                else
                {
                    _panels[i].SetActive(false);
                }
            }
        }

        public void SetPanel(int index)
        {
            _panelIndex = index;
            // Debug.Log($"[Panel] Showing{_panelIndex}:{_panels[_panelIndex].name}");
            ShowCurrentPanel();
        }
    }
}