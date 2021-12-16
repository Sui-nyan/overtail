using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Overtail;
[RequireComponent(typeof(PlayerMovement))]
public class Player : MonoBehaviour
{
    PlayerMovement movement;
    [SerializeField] private PersistentPlayerData persistantData;

    private void Start()
    {
        gameObject.transform.position = persistantData.playerSerializable.Position;
        movement = gameObject.GetComponent<PlayerMovement>();
    }
    void FixedUpdate()
    {
        if (movement.IsMoving) persistantData.playerSerializable.Position = gameObject.transform.position;
    }
}
