using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogueResposeEvents))]

public class DialogueResponseEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        {
            DrawDefaultInspector();

            DialogueResposeEvents resposeEvents = (DialogueResposeEvents)target;

            if (GUILayout.Button("Refresh"))
            {
                resposeEvents.OnValidate();
            }
        }
    }
}
