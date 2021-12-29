using System.Collections;
using UnityEngine;
using System;

namespace Overtail.Arcade.Street
{
    public class Player : MonoBehaviour
    {
        public event Action<Car> GotRunOver;
        public event Action ReachedGoal;

        public float speed = 1f;

        private Rigidbody2D rigidBody;
        private BoxCollider2D boxCollider;

        private Vector2 facing;

        private Vector3 startPosition;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Car c;
            if (collision.TryGetComponent<Car>(out c))
            {
                OnHitByCar(c);
                return;
            }

            Goal g;
            if (collision.TryGetComponent<Goal>(out g))
            {
                OnReachGoal(g);
                return;
            }
        }

        private void OnReachGoal(Goal g)
        {
            SpriteRenderer g_sr;
            if (g.TryGetComponent<SpriteRenderer>(out g_sr))
            {
                g_sr.color = new Color(0, 1, 1, 0.5f);
            }

            SpriteRenderer p_sr;
            if (this.TryGetComponent<SpriteRenderer>(out p_sr))
            {
                p_sr.color = new Color(0, 1, 0, .5f);
            }

            ReachedGoal?.Invoke();
        }

        private void OnHitByCar(Car c)
        {
            SpriteRenderer spr;
            if (this.gameObject.TryGetComponent<SpriteRenderer>(out spr))
            {
                spr.color = Color.red;
            }
            GotRunOver?.Invoke(c);
        }

        // Use this for initialization
        void Awake()
        {
            //rigidBody = this.gameObject.GetComponent<Rigidbody2D>();
            rigidBody = this.gameObject.AddComponent<Rigidbody2D>();
            rigidBody.freezeRotation = true;
            rigidBody.gravityScale = 0;

            boxCollider = this.gameObject.AddComponent<BoxCollider2D>();
            boxCollider.isTrigger = true;

            startPosition = rigidBody.position;
        }

        private void Update()
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            facing = new Vector2(x, y);
        }

        internal void Reset()
        {
            this.rigidBody.position = startPosition;
            SpriteRenderer spr;
            if (this.gameObject.TryGetComponent<SpriteRenderer>(out spr))
            {
                spr.color = Color.white;
            }
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            if (Time.timeScale < 2) rigidBody.MovePosition(rigidBody.position += facing.normalized * speed * Time.fixedDeltaTime);
        }
    }
}