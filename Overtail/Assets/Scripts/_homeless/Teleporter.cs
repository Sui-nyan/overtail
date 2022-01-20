using System;
using Overtail.PlayerModule;
using UnityEditor;
using UnityEngine;

[DisallowMultipleComponent]
public class Teleporter : MonoBehaviour
{
    public Vector2 targetPos;
    public GameObject targetObject;

    [SerializeField] internal bool usePosition;
    [SerializeField] internal bool useDefaultCollider;

    private Collider2D _collider;

    void Awake()
    {
        if (useDefaultCollider)
        {
            if (TryGetComponent<Collider2D>(out var currentCollider))
            {
                Destroy(currentCollider);
            }

            _collider = gameObject.AddComponent<BoxCollider2D>();
            _collider.isTrigger = true;
        }
        else
        {
            _collider = GetComponent<Collider2D>();
            if (_collider is null)
            {
                Debug.LogError("<color=red>Teleporter is missing 2D collider</color>");
                throw new ArgumentNullException();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var obj = other.gameObject;
        Debug.Log("Collision with" + obj);
        if (obj.TryGetComponent<Player>(out _))
        {
            var pos = obj.transform.position;

            if (usePosition)
            {
                pos.x = targetPos.x;
                pos.y = targetPos.y;
            }
            else
            {
                var dest = targetObject.transform.position;
                pos.x = dest.x;
                pos.y = dest.y;
            }

            obj.transform.position = pos;
        }
    }
}

[CanEditMultipleObjects]
[CustomEditor(typeof(Teleporter))]
public class TeleporterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        var tp = (Teleporter)target;

        var type = (DestinationType)EditorGUILayout
            .EnumPopup("Destination Type", tp.usePosition
                ? DestinationType.Position
                : DestinationType.GameObject);

        tp.usePosition = type == DestinationType.Position;

        // USE TARGET OBJECT
        using (new EditorGUI.DisabledScope(tp.usePosition == true))
        {
            EditorGUILayout.BeginHorizontal();
            tp.targetObject = (GameObject)EditorGUILayout
                .ObjectField("Destination", tp.targetObject, typeof(GameObject), true);

            if (GUILayout.Button("Create", GUILayout.ExpandWidth(false)))
            {
                CreateObject(tp);
            }

            EditorGUILayout.EndHorizontal();
            if (EditorUtility.IsPersistent(tp.targetObject))
            {
                EditorGUILayout.HelpBox("The object might not be a scene object.", MessageType.Warning);
            }
        }

        // USE TARGET POS VECTOR2
        using (new EditorGUI.DisabledScope(tp.usePosition == false))
        {
            tp.targetPos = EditorGUILayout.Vector2Field("Position", tp.targetPos);
        }

        tp.useDefaultCollider = EditorGUILayout.Toggle("Use default collider", tp.useDefaultCollider);

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


        if (!tp.usePosition && tp.targetObject == null) return;
        Vector2 dest = tp.usePosition || tp.targetObject is null ? tp.targetPos : (Vector2)tp.targetObject.transform.position;

        Handles.color = color;
        Handles.DrawDottedLine(tp.transform.position, dest, 10);



        // Draw Handle for position
        if (!tp.usePosition) return;
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
    enum DestinationType
    {
        GameObject,
        Position
    }
}
