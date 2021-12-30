using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable, INPC
{
    [SerializeField] string NPCName;
    [SerializeField] Sprite portrait;
    [SerializeField] Sprite Sprite;

    Sprite INPC.Sprite { get; set; }

    private Rigidbody2D rb;
    public float moveSpeed;
    bool isWalking;
    private int directions;
    private float waitCounter;
    public float waitTime;
    private float walkCounter;
    public float walkTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        waitCounter = waitTime;
        walkCounter = walkTime;
        moveDirections();
    }

    void Update()
    {
        movement();
        Debug.Log(isWalking);
        Debug.Log(directions);
        Debug.Log(rb.velocity);
    }

    public void movement()
    {
        if (isWalking)
        {
            walkCounter -= Time.deltaTime;

            switch (directions)
            {
                case 0:
                    rb.velocity = new Vector2(-moveSpeed, 0);
                    break; //left
                case 1:
                    rb.velocity = new Vector2(moveSpeed, 0);
                    break; //right
                case 2:
                    rb.velocity = new Vector2(0, moveSpeed);
                    break; //up
                case 3:
                    rb.velocity = new Vector2(0, -moveSpeed);
                    break; //down
            }

            if (walkCounter < 0)
            {
                isWalking = false;
                waitCounter = waitTime;
            }
        }

        else
        {
            waitCounter -= Time.deltaTime;

            if(waitCounter < 0)
            {
                moveDirections();
            }
        }
        
    }

    public void moveDirections()
    {
        directions = Random.Range(0, 4);
        isWalking = true;
    }

    public void Intectact(Player player)
    {
        
    }
}
