using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rb;
    public Vector2 movement;
    public Animator animator;

    public CharState state;

    public bool IsMoving { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition = state.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if (movement.x != 0 || movement.y != 0) {
            if (movement.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            if (movement.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }

            IsMoving = true;
            animator.SetBool("isWalking", true);
            rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
            state.UpdatePosition(transform.localPosition);
            Debug.Log(state.position);
        } else {
            IsMoving = false;
            animator.SetBool("isWalking", false);
        }


    }
}
