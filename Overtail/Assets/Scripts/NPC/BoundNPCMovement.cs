using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overtail.NPCs
{
    public class BoundNPCMovement : MonoBehaviour
    {
        [SerializeField] Sprite portrait;

        private Rigidbody2D NPCBody;
        private Transform transform;
        public Collider2D boundary;

        public float moveSpeed;
        private bool isWalking;
        private bool playerInRange;

        private int directions;
        private Vector3 directionVector;

        private float waitCounter;
        public float maxWaitTime;
        private float walkCounter;
        public float maxWalkTime;


        void Start()
        {
            NPCBody = GetComponent<Rigidbody2D>();
            transform = GetComponent<Transform>();

            waitCounter = Random.Range(0, maxWaitTime);
            walkCounter = Random.Range(0, maxWalkTime);
            changeDirection();
        }

        void Update()

        {
            if (isWalking)
            {
                walkCounter -= Time.deltaTime;
                if (walkCounter <= 0)
                {
                    walkCounter = maxWalkTime;
                    isWalking = false;
                }
                movement();
            }
            else
            {
                waitCounter -= Time.deltaTime;
                if (waitCounter <= 0)
                {
                    isWalking = true;
                    waitCounter = maxWaitTime;
                    changeDirection();
                }
            }

        }

        public void movement()
        {
            Vector3 temp = transform.position + directionVector * moveSpeed * Time.deltaTime;
            if (boundary.bounds.Contains(temp))
            {
                NPCBody.MovePosition(temp);
            }
            else
            {
                changeDirection();
            }

        }

        public void changeDirection() //NPC will randomly change their movement direction
        {
            directions = Random.Range(0, 4);
            isWalking = true;

            if (isWalking)
            {


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
    }
}

