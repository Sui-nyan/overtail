using Overtail.Items.Components;
using UnityEditor;
using UnityEngine;

namespace Overtail.Items
{
    [CustomEditor(typeof(Lootable))]
    public class LootableEditor : Editor
    {
        private Item item;

        private int index = -1;

        public override void OnInspectorGUI()
        {
            var allIds = ItemDatabase.GetAllIds();

            var loot = target as Lootable;
            
            index = EditorGUILayout.Popup("Item", index, allIds.ToArray());

            if (index < 0)
            {
                EditorGUILayout.HelpBox("Error Index: " + index, MessageType.Error);
                using (new EditorGUI.DisabledScope(true))
                {
                    base.OnInspectorGUI();
                }
                return;
            }

            var val = EditorGUILayout.IntField("Quantity", loot.stack.Quantity);

            item = ItemDatabase.GetFromIndex(index);
            loot.stack.Item = item;

            var maxQuantity = item.GetComponent<StackComponent>()?.MaxQuantity ?? 1;

            loot.stack.Quantity = Mathf.Clamp(val, 1, maxQuantity);

            loot.GetComponent<SpriteRenderer>().sprite = loot.stack.Item.Sprite;
            loot.name = loot.stack.Item.Name;


            using (new EditorGUI.DisabledScope(true))
            {
                base.OnInspectorGUI();
            }
        }
    }
}