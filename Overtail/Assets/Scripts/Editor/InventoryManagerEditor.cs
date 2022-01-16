using Overtail.Items.Components;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Overtail.Items
{
    [CustomEditor(typeof(InventoryManager))]
    public class InventoryManagerEditor : Editor
    {
        private int index = -1;
        private Item item;
        private int quantity;

        private bool? result = null;

        private bool editMode = false;

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical("Box");

            var allIds = ItemDatabase.GetAllIds();


            index = EditorGUILayout.Popup("Item", index, allIds.ToArray());

            if (index >= 0)
            {
                item = ItemDatabase.GetFromIndex(index);

                var quantityField = EditorGUILayout.IntField("Quantity", quantity);

                var maxQuantity = item.GetComponent<StackComponent>()?.MaxQuantity ?? 1;

                quantity = Mathf.Clamp(quantityField, 1, maxQuantity);
            }

            using (new EditorGUI.DisabledScope(index < 0))
            {
                if (GUILayout.Button("Add Item to inventory"))
                {
                    result = AddToInventory();
                }
            }

            if (result != null)
            {
                if (result == true)
                {
                    EditorGUILayout.HelpBox("Added successfully", MessageType.Info);
                }
                else
                {
                    EditorGUILayout.HelpBox("Inventory capacity exceeded", MessageType.Warning);
                }
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            editMode = EditorGUILayout.ToggleLeft("Enable Editing", editMode, GUILayout.ExpandWidth(false));
            EditorGUILayout.EndHorizontal();
            using (new EditorGUI.DisabledScope(editMode == false))
            {
                base.OnInspectorGUI();
            }
        }

        private bool AddToInventory()
        {
            var manager = target as InventoryManager;
            return manager.PickUp(new ItemStack(item, quantity));
        }
    }
}