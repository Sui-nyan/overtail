using System.Collections;
using System.Collections.Generic;
using Overtail.PlayerModule;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Assigns a render order to each element in the scene in respect to the player. 
/// </summary>
public class PositionalRenderSorter : MonoBehaviour
{
    [SerializeField]
    private bool _runOnlyOnce = false;

    private float _timer;
    private float _timerMax = .1f;

    private Player _player;
    [SerializeField] private int _sortingOrderBase;
    [SerializeField] private string sortingLayer = "Foreground";

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
        _sortingOrderBase = _player?.GetComponent<SpriteRenderer>()?.sortingOrder ?? 0;
    }

    private void LateUpdate()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
            _timer = _timerMax;

        SetSortingOrder(this.transform);

        if (_runOnlyOnce)
        {
            Destroy(this);
        }
    }
    
    private void SetSortingOrder(Transform t)
    {
        if (t.gameObject.TryGetComponent<Renderer>(out var r))
        {
            r.sortingOrder = Mathf.CeilToInt(_sortingOrderBase - t.transform.position.y + _player.transform.position.y);
            r.sortingLayerName = sortingLayer;
        }

        foreach (Transform child in t)
        {
            SetSortingOrder(child);
        }
    }
}
