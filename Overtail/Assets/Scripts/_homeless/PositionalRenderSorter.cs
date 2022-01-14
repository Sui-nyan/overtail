using System.Collections;
using System.Collections.Generic;
using Overtail.PlayerModule;
using UnityEngine;

// TODO namespace
public class PositionalRenderSorter : MonoBehaviour
{
    [SerializeField]
    private bool _runOnlyOnce = false;

    private float _timer;
    private float _timerMax = .1f;

    private Player _player;
    private int _sortingOrderBase;

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

        foreach (Transform child in transform)
        {
            SetSortingOrder(child);
        }

        if (_runOnlyOnce)
        {
            Destroy(this);
        }
    }

    private void SetSortingOrder(Transform child)
    {
        if (child.gameObject.TryGetComponent<Renderer>(out var r))
        {
            r.sortingOrder = (int) (_sortingOrderBase - child.transform.position.y + _player.transform.position.y);
        }
    }
}
