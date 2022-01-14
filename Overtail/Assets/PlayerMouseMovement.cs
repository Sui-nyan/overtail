using System.Collections;
using System.Collections.Generic;
using Overtail.PlayerModule;
using UnityEngine;

public class PlayerMouseMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private Vector2 direction;
    private Vector2 mousePos;


    //tmp
    public float speed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); 
    }

    void Update()
    {
        Debug.DrawLine(rb.position, mousePos);

        if (Input.GetMouseButton(0)) SetDirection();
    }

    private void SetDirection()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var pos = ray.origin;
        mousePos.x = pos.x;
        mousePos.y = pos.y;

        var v = mousePos - rb.position;
        direction = v.normalized;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        direction = Vector2.zero;
    }
}
