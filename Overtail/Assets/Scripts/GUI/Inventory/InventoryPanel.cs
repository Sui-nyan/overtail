using System.Linq;
using Overtail.Items;
using Overtail.PlayerModule;
using Overtail.Util;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Overtail.GUI
{
    public class InventoryPanel : Panel
    {
        public GameObject Grid;
        public GameObject EquipmentSlot;

        private InventorySlot[] _itemSlots;
        private InventorySlot _mainHand;
        private InventorySlot _offHand;

        internal GameObject subMenu { get; set; }

        private InventoryManager _inventoryManager;

        private GameObject _lastSelection;


        void Awake()
        {
            _inventoryManager = GameObject.FindObjectOfType<InventoryManager>();

            _itemSlots = Grid.GetComponentsInChildren<InventorySlot>().Select(b => b).ToArray();

            var eq = EquipmentSlot.GetComponentsInChildren<InventorySlot>().Select(b => b).ToArray();
            _mainHand = eq[0];
            _offHand = eq[1];
        }

        void Start()
        {
            SetNavigation();
        }

        void Update()
        {
            var selection = EventSystem.current.currentSelectedGameObject;

            if (GameObjectTree.ContainsGameObject(this.gameObject, selection))
            {
                _lastSelection = selection;
            }


            LoadFromInventory();
            LoadFromPlayer();
        }


        private void SetNavigation()
        {
            var parent = Grid;
            var rows = parent.transform.childCount;
            var columns = parent.transform.GetChild(0).childCount;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    var button = parent.transform.GetChild(i).GetChild(j).GetComponentInChildren<Button>();
                    var nav = button.navigation;
                    nav.mode = Navigation.Mode.None;
                    button.navigation = nav;

                    nav.mode = Navigation.Mode.Explicit;
                    nav.selectOnRight = parent.transform
                        .GetChild(i)
                        .GetChild((j + 1) % columns).GetComponentInChildren<Button>();
                    nav.selectOnLeft = parent.transform
                        .GetChild(i)
                        .GetChild((columns + j - 1) % columns).GetComponentInChildren<Button>();

                    nav.selectOnUp = parent.transform
                        .GetChild((rows + i - 1) % rows)
                        .GetChild(j).GetComponentInChildren<Button>();
                    nav.selectOnDown = parent.transform
                        .GetChild((i + 1) % rows)
                        .GetChild(j).GetComponentInChildren<Button>();


                    // last row
                    if (i == (rows - 1))
                    {
                        // two outer right slots
                        if (j == columns - 2)
                        {

                            nav.selectOnDown = _mainHand.GetComponentInChildren<Button>();
                        }

                        if (j == columns - 1)
                        {
                            nav.selectOnDown = _offHand.GetComponentInChildren<Button>();
                        }
                    }

                    button.navigation = nav;
                }
            }
        }

        private void SetNavigationLeftExit()
        {
            // TODO generalize
            // Left Navigation for out left column
            foreach (Transform row in transform)
            {
                var button = row.GetComponentInChildren<InventorySlot>().GetComponent<Button>();

                // Add Event Trigger to left navigation
                if (!button.TryGetComponent<EventTrigger>(out var trigger))
                    trigger = button.gameObject.AddComponent<EventTrigger>();

                EventTrigger.Entry entry = new EventTrigger.Entry();

                entry.eventID = EventTriggerType.Move;
                entry.callback.AddListener((eventData) =>
                {
                    var e = (AxisEventData)eventData;
                    if (e.moveDir == MoveDirection.Left)
                    {
                        FindObjectOfType<PanelGroup>().ExitUI();
                        FindObjectOfType<TabGroup>().EnterUI();
                    }
                });

                trigger.triggers.Add(entry);
            }
        }

        public override void EnterUI()
        {
            EventSystem.current.SetSelectedGameObject(_lastSelection is null ? _itemSlots[0].gameObject : _lastSelection);
        }

        public void Refocus()
        {
            bool focusOnObjectOutside = !GameObjectTree
                .ContainsGameObject(this.gameObject, EventSystem.current.currentSelectedGameObject);
            if (focusOnObjectOutside)
            {
                EnterUI();
            }
        }

        public override void ExitUI()
        {

        }

        private void LoadFromPlayer()
        {
            Player p = GameObject.FindObjectOfType<Player>();

            _mainHand.Stack = p.MainHand != null ? new ItemStack(p.MainHand, 1) : null;
            _offHand.Stack = p.OffHand != null ? new ItemStack(p.OffHand, 1) : null;
        }

        private void LoadFromInventory()
        {
            var stacks = _inventoryManager.GetItems();

            for (int i = 0; i < _itemSlots.Length; i++)
            {
                _itemSlots[i].Stack = i < stacks.Count ? stacks[i] : null;
            }
        }

        public void CloseSubMenu()
        {
            if (subMenu == null) return;
            Destroy(subMenu);
            EventSystem.current.SetSelectedGameObject(_lastSelection);
        }

        public void SetSubMenu(GameObject newSubMenu)
        {
            _lastSelection = EventSystem.current.currentSelectedGameObject;
            CloseSubMenu();
            subMenu = newSubMenu;
        }
    }
}
