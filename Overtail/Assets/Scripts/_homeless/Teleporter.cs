using System;
using Overtail.PlayerModule;
using UnityEngine;

/// <summary>
/// Teleports Player to prespecified position on entering collision box. 
/// </summary>
[DisallowMultipleComponent]
public class Teleporter : MonoBehaviour
{
    public Vector2 targetPos;

    [SerializeField] public bool useDefaultCollider;

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

            pos.x = targetPos.x;
            pos.y = targetPos.y;

            obj.transform.position = pos;
        }
    }
}