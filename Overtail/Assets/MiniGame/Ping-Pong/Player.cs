using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPong : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rg;
    public bool left;
    void Update()
    {
        if (left)
        {
            if(Input.GetKey(KeyCode.W))
            {
                rg.velocity = new Vector2(0f, speed);
            } else if (Input.GetKey(KeyCode.S))
            {
                rg.velocity = new Vector2(0f, -speed);
            } else 
            {
                rg.velocity = new Vector2(0f, 0f);
            }
        } else
        {
            if (left)
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    rg.velocity = new Vector2(0f, speed);
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    rg.velocity = new Vector2(0f, -speed);
                }
                else
                {
                    rg.velocity = new Vector2(0f, 0f);
                }
            }
        }
    }
}
