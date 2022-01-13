using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Overtail.GUI
{
    public class InventoryPanel : Panel
    {

        public GameObject Grid;
        public GameObject EquipmentSlot;

        private GameObject[] _itemSlots;
        private GameObject _leftSlot;
        private GameObject _rightSlot;

        public void Awake()
        {
            _itemSlots = Grid.GetComponentsInChildren<Button>().Select(b => b.gameObject).ToArray();
            var eq = EquipmentSlot.GetComponentsInChildren<Button>().Select(b => b.gameObject).ToArray();
            _leftSlot = eq[0];
            _rightSlot = eq[1];
        }

        private void PopulateSlots()
        {
            throw new System.NotImplementedException();
        }

        public override void Refresh()
        {
            for (int i = 0; i < _itemSlots.Length; i++)
            {
                _itemSlots[i].name = $"Item {i}";
            }

            _leftSlot.name = "Left Hand";
            _rightSlot.name = "Right Hand";
        }
    }
}