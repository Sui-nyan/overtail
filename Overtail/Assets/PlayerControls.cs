using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float boundY = 2.25f;
    private Rigidbody2D rb2d;

    private GameObject ball;
    private Vector2 ballPos;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    void Move()
    {
        if (!ball)
        {
            ball = GameObject.FindGameObjectWithTag("ball");
        }
        if (ball.GetComponent<BallControl> ().ballDirection == Vector2.right)
        {

        }
    }
}
