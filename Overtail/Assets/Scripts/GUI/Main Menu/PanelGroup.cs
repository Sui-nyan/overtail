using System.Linq;
using UnityEngine;

namespace Overtail.GUI
{
    /// <summary>
    /// Group of menu panels. Handles switching between panels and navigation
    /// </summary>
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

        public void EnterUI()
        {
            var p = _panels[_panelIndex].GetComponent<Panel>();
            p.EnterUI();
        }

        public void ExitUI()
        {
            var p = _panels[_panelIndex].GetComponent<Panel>();
            p.ExitUI();
        }

        private void UpdatePanelVisibility()
        {
            for (int i = 0; i < _panels.Length; i++)
            {
                if (i == _panelIndex)
                {
                    _panels[i].SetActive(true);
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
            UpdatePanelVisibility();
        }
    }
}
