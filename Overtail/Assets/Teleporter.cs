using System;
using System.Collections;
using System.Collections.Generic;
using Overtail.PlayerModule;
using UnityEditor;
using UnityEngine;

[DisallowMultipleComponent]
public class Teleporter : MonoBehaviour
{
    public Vector2 destination;
    public GameObject destinationObject;

    internal bool usePosition = true;
    internal bool useDefaultCollider;

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
            pos.x = destination.x;
            pos.y = destination.y;

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
        var tp = (Teleporter)target;

        var type = (DestinationType) EditorGUILayout
            .EnumPopup("Destination Type", tp.usePosition 
                ? DestinationType.Position 
                : DestinationType.GameObject);

        tp.usePosition = type == DestinationType.Position;

        using (new EditorGUI.DisabledScope(tp.usePosition == true))
        {
            tp.destinationObject = (GameObject)EditorGUILayout
                .ObjectField("Destination", tp.destinationObject, typeof(GameObject), true);
            if (EditorUtility.IsPersistent(tp.destinationObject))
            {
                EditorGUILayout.HelpBox("The object might not be a scene object.", MessageType.Warning);
            }
        }
        using (new EditorGUI.DisabledScope(tp.usePosition == false))
        {
            tp.destination = EditorGUILayout.Vector2Field("Position", tp.destination);
        }

        tp.useDefaultCollider = EditorGUILayout.Toggle("Use default collider", tp.useDefaultCollider);

        if (!EditorApplication.isPlaying
            && tp.useDefaultCollider
            && tp.gameObject.TryGetComponent<Collider2D>(out _))
        {
            EditorGUILayout.HelpBox("Current collider will be overwritten", MessageType.Warning);
        }
    }

    enum DestinationType
    {
        GameObject,
        Position
    }
}

