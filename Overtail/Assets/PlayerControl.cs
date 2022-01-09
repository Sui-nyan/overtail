using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour

{
    public float moveSpeed = 10.0f;
    public float topBounds = 2.25f;
    public float bottomBounds = -2.25f;
    private Rigidbody2D rb2d;
    public Vector2 startingPosition = new Vector2(13.0f, 0.0f);

    private GameObject ball;
    private Vector2 ballPos;
    
    void Start()
    {
        transform.localPosition = (Vector3)startingPosition;
        
    }

    void Update()
    {
        CheckUserInput ();
    }

    void CheckUserInput ()
    {
        if (Input.GetKey (KeyCode.W))
        {
            if (transform.localPosition.y > topBounds)
            {
                transform.localPosition = new Vector3 (transform.localPosition.x, bottomBounds, transform.localPosition.z);
            }
            else 
            {
            transform.localPosition += Vector3.up * moveSpeed * Time.deltaTime;
            }
        }
        else if (Input.GetKey (KeyCode.S))
        {
            if (transform.localPosition.y < bottomBounds)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, bottomBounds, transform.localPosition.z);
            }
            else
            {
                transform.localPosition += Vector3.down * moveSpeed * Time.deltaTime;
            }
        }
    }
 }
