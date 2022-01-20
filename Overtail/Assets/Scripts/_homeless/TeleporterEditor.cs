using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(Teleporter))]
public class TeleporterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var tp = (Teleporter)target;

        if (!EditorApplication.isPlaying
            && tp.useDefaultCollider
            && tp.gameObject.TryGetComponent<Collider2D>(out _))
        {
            EditorGUILayout.HelpBox("Current collider will be overwritten", MessageType.Warning);
        }
    }

    private static void CreateObject(Teleporter tp)
    {
        var go = new GameObject();
        go.transform.SetParent(tp.transform);

        go.name = "Teleport Destination";
        go.tag = "EditorOnly";
        go.transform.position = tp.transform.position;

        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Square");
        sr.color = Color.red;
        sr.sortingLayerName = "UI";


        tp.targetObject = go;
    }

    public void OnSceneGUI()
    {
        // Settings
        var color = Color.red;

        var tp = target as Teleporter;

        Handles.color = color;
        Handles.DrawDottedLine(tp.transform.position, tp.targetPos, 10);

        // Draw Handle for position
        float size = HandleUtility.GetHandleSize(tp.targetPos) * 0.2f;
        Vector2 snap = Vector3.one * 0.5f;

        EditorGUI.BeginChangeCheck();
        Handles.color = color;
        Vector3 newTargetPosition = Handles.FreeMoveHandle(tp.targetPos, Quaternion.identity, size, snap, Handles.RectangleHandleCap);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(tp, "Change Look At Target Position");
            tp.targetPos = newTargetPosition;
        }
    }
}