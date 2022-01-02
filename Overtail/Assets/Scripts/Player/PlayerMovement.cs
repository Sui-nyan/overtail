using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private DialogueManager dialogueManager;

    public DialogueManager DialogueManager => dialogueManager;

    public IInteractable interactable { get; set; }
    public bool IsMoving { get; private set; }

    public float moveSpeed;
    private Rigidbody2D rb;
    private Vector3 change;
    public Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");

        if (!DialogueManager.IsOpen)
        {
            MoveCharacter();
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
        }
        

        Debug.Log("moveX: " + animator.GetFloat("moveX") + " moveY: " + animator.GetFloat("moveY"));

        if (Input.GetKeyDown(KeyCode.E) && !DialogueManager.IsOpen)
        {
            interactable?.Intectact(this);
        }
    }

    public void MoveCharacter()
    {
        if(change != Vector3.zero)
        {
            rb.MovePosition(transform.position + change * moveSpeed * Time.deltaTime);
            animator.SetBool("isWalking", true);
        } else
        {
            animator.SetBool("isWalking", false);
        }
        
    }
}
