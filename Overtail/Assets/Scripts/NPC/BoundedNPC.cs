using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundedNPC : MonoBehaviour, IInteractable
{
    [SerializeField] Sprite portrait;

    private Rigidbody2D NPCBody;
    private Transform transform;
    public Collider2D boundary;

    public float moveSpeed;
    bool isWalking;
    bool playerInRange;

    private int directions;
    private Vector3 directionVector;

    private float waitCounter;
    public float waitTime;
    private float walkCounter;
    public float walkTime;

    bool isWithinBonds;


    void Start()
    {
        NPCBody = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
        
        waitCounter = waitTime;
        walkCounter = walkTime;
        changeDirection();


    }

    void Update()

    {
        movement();
        Debug.Log(isWithinBonds);
    }

    public void movement()
    {
        Vector3 temp = transform.position + directionVector * moveSpeed * Time.deltaTime;
        if (boundary.bounds.Contains(temp))
        {
            isWithinBonds = true;
            NPCBody.MovePosition(temp);
        }
        else
        {
            isWithinBonds = false;
            changeDirection();
        }
        
    }

    public void changeDirection() //NPC will randomly change their movement direction
    {
        directions = Random.Range(0, 4);
        isWalking = true;

        if (isWalking)
        {
            walkCounter -= Time.deltaTime;

            switch (directions)
            {
                case 0:
                    directionVector = Vector3.left;
                    break; //Walk left
                case 1:
                    directionVector = Vector3.right;
                    break; //Walk right
                case 2:
                    directionVector = Vector3.up;
                    break; //Walk up
                case 3:
                    directionVector = Vector3.down;
                    break; //Walk down
                default: break;
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

            if (waitCounter < 0)
            {
                changeDirection();
            }
        }
    }

    public void stopMovement()
    {
        NPCBody.velocity = Vector2.zero;
        isWalking = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Vector3 temp = directionVector;
        changeDirection();
        int loop = 0;
        while (temp == directionVector && loop < 10)
        {
            loop++;
            changeDirection();
        }
    }

    public void Intectact(PlayerMovement player)
    {
        
    }
}
