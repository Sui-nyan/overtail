using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPC : MonoBehaviour, IInteractable, INPC
{
    [SerializeField] string NPCName;
    [SerializeField] Sprite portrait;
    [SerializeField] Sprite Sprite;

    Sprite INPC.Sprite { get; set; }

    private Rigidbody2D rb;
    private float moveSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void movement()
    {

    }

    public void Intectact(Player player)
    {
        
    }
}
