using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundedNPC : MonoBehaviour, IInteractable
{
    [SerializeField] Sprite portrait;

    private Rigidbody2D rb;
    private new Transform transform;
    public Collider2D boundary;

    public float moveSpeed;
    bool isWalking;
    bool playerInRange;

    private Vector2 minWalkBound;
    private Vector2 maxWalkBound;

    private int directions;
    private int directionVector;

    private float waitCounter;
    public float waitTime;
    private float walkCounter;
    public float walkTime;

    


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
        
        waitCounter = waitTime;
        walkCounter = walkTime;
        moveDirections();

        minWalkBound = boundary.bounds.min;
        maxWalkBound = boundary.bounds.max;

    }

    void Update()
    {
        if (!playerInRange)
        {
            movement();
        }
        
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
                    if (transform.position.x > minWalkBound.x)
                    {
                        isWalking = false;
                        waitCounter = waitTime;
                    }
                    break; //left
                case 1:
                    rb.velocity = new Vector2(moveSpeed, 0);
                    if (transform.position.x > maxWalkBound.x)
                    {
                        isWalking = false;
                        waitCounter = waitTime;
                    }
                    break; //right
                case 2:
                    rb.velocity = new Vector2(0, moveSpeed);
                   if (transform.position.y > maxWalkBound.y)
                    {
                        isWalking = false;
                        waitCounter = waitTime;
                    }
                    break; //up
                case 3:
                    rb.velocity = new Vector2(0, -moveSpeed);
                    if (transform.position.y > minWalkBound.x)
                    {
                        isWalking = false;
                        waitCounter = waitTime;
                    }
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

    public void stopMovement()
    {
        rb.velocity = Vector2.zero;
        isWalking = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        playerInRange = true;
    }

    public void Intectact(Player player)
    {
        
    }
}
