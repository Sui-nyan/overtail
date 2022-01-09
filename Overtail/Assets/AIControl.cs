using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControl : MonoBehaviour
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
        rb2d = GetComponent<Rigidbody2D>();
    }

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
            ballPos = ball.transform.localPosition;

            if (transform.localPosition.y > bottomBounds && ballPos.y < transform.localPosition.y)
            {
                transform.localPosition += new Vector3(0, -moveSpeed * Time.deltaTime, 0);
            }
            if (transform.localPosition.y < topBounds && ballPos.y > transform.localPosition.y)
            {
                transform.localPosition += new Vector3(0, moveSpeed * Time.deltaTime, 0);
            }
        }
    }
}
