using UnityEditor;
using UnityEngine;

namespace Overtail.GUI
{
    [CustomEditor(typeof(InventoryPanel))]
    public class PanelEditor : Editor
    {
        //private SerializedObject panel;

        void OnEnable()
        {
            
        }

        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            if(target as Panel == null) EditorGUILayout.HelpBox("Panel is NULL", MessageType.Error);

            if(target.GetType() == typeof(InventoryPanel))
            {
                
                if (GUILayout.Button("Set Children Names"))
                {
                    var grid = (GameObject)serializedObject.FindProperty("Grid").objectReferenceValue;
                    
                    var equipmentGroup = (GameObject)serializedObject.FindProperty("EquipmentSlot").objectReferenceValue;
                    var eqNames = new string[] { "MainHand", "OffHand", "Helmet", "Armor" };

                    int i = 0;
                    foreach (Transform row in grid.transform)
                    {
                        foreach (Transform slot in row)
                        {
                            if (slot.TryGetComponent<InventorySlot>(out _))
                            {
                                slot.name = "Slot:" + i++;
                            }
                        }
                    }

                    var eq = equipmentGroup.GetComponentsInChildren<InventorySlot>();
                    for (int j = 0; j < eq.Length && j < eqNames.Length; j++)
                    {
                        eq[j].name = eqNames[j];
                    }
                }
                EditorGUILayout.HelpBox("Make sure to enable all child game objects", MessageType.Warning);
            }
        }

    }
}