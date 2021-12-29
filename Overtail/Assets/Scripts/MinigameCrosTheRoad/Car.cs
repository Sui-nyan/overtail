using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public event Action<Car> OnCrash;

    public static float globalSpeedMultiplier = 1;

    public float velocity = 1;
    public Vector2 facing = new Vector2(1, 0);
    public float maxDistance;

    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private float distanceTraveled;

    public Vector2 Size { get => bc.size; set => bc.size = value; }

    private void Awake()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        bc = this.gameObject.GetComponent<BoxCollider2D>();
    }


    // Update is called once per frame
    private void Update()
    {
        Move();
        if (IsExpired())
        {
            Destroy(this.gameObject);
        }
    }

    private bool IsExpired()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) return true;

        return distanceTraveled > maxDistance;
    }

    private void Move()
    {
        Vector2 dv = facing.normalized * velocity * globalSpeedMultiplier;
        distanceTraveled += dv.magnitude;
        rb.MovePosition(rb.position + dv);
    }
}
