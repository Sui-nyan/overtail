using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private DialogueManager dialogueManager;

    public DialogueManager DialogueManager => dialogueManager;

    public IInteractable interactable { get; set; }

    public float moveSpeed;
    private Rigidbody2D rb;
    public Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!DialogueManager.IsOpen)
        {
            PlayerMovement();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            interactable?.Intectact(this);
        }
    }

    public void PlayerMovement()
    {
        Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (movement.x != 0 || movement.y != 0)
        {
            if (movement.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            if (movement.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }

            animator.SetBool("isWalking", true);
            rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }
}
