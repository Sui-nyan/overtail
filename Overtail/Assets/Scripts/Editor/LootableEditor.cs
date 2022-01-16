using Overtail.Items.Components;
using UnityEditor;
using UnityEngine;

namespace Overtail.Items
{
    [CustomEditor(typeof(Lootable))]
    public class LootableEditor : Editor
    {
        private int index = -1;
        private Item item;
        public override void OnInspectorGUI()
        {
            var allIds = ItemDatabase.GetAllIds();

            var loot = target as Lootable;

            index = allIds.IndexOf(loot?.stack?.Item.Id);

            index = EditorGUILayout.Popup("Item", index, allIds.ToArray());

            if (index < 0)
            {
                EditorGUILayout.HelpBox("Error Index: " + index, MessageType.Error);
                base.OnInspectorGUI();
                return;
            }

            item = ItemDatabase.GetFromIndex(index);

            loot.stack.Item = item;
            
            var val = EditorGUILayout.IntField("Quantity", loot.stack.Quantity);

            var maxQuantity = item.GetComponent<StackComponent>()?.MaxQuantity ?? 1;

            loot.stack.Quantity = Mathf.Clamp(val, 1, maxQuantity);

            using (new EditorGUI.DisabledScope(true))
            {
                base.OnInspectorGUI();
            }
        }
    }
}