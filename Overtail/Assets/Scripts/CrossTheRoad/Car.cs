using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overtail.Arcade.Street
{

    public class Car : MonoBehaviour
    {
        public static float globalSpeedMultiplier = 1;

        public float velocity;
        public Vector2 facing;
        public float maxDistance;

        private Rigidbody2D rb;
        private BoxCollider2D bc;
        private float distanceTraveled;

        public Vector2 Size { get => bc.size; set => bc.size = value; }

        private void Awake()
        {
            this.gameObject.transform.localScale = new Vector3(2, 1, 1);
            rb = this.gameObject.AddComponent<Rigidbody2D>();
            rb.freezeRotation = true;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation; ;
            rb.gravityScale = 0;

            bc = this.gameObject.AddComponent<BoxCollider2D>();
        }

        // Update is called once per frame
        private void FixedUpdate()
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
            Vector2 dv = facing.normalized * velocity * globalSpeedMultiplier * Time.fixedDeltaTime;
            distanceTraveled += dv.magnitude;
            rb.MovePosition(rb.position + dv);
        }
    }
}