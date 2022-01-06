using UnityEngine;
using Overtail;

[RequireComponent(typeof(PlayerMovement))]
public class Player : MonoBehaviour
{
    private PlayerMovement _movement;
    [SerializeField] private PlayerData _playerData;

    private void Awake()
    {
        var t = transform.position;
        t.x = _playerData.Position.x;
        t.y = _playerData.Position.y;
        transform.position = t;

        _movement = GetComponent<PlayerMovement>();
    }
    void FixedUpdate()
    {
        if (_movement.IsMoving)
            _playerData.playerSerializable.Position= gameObject.transform.position;
    }
}
