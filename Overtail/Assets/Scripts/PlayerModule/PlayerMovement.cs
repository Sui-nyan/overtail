using UnityEngine;

namespace Overtail.PlayerModule
{
    [RequireComponent(typeof(Rigidbody2D))]
    [DisallowMultipleComponent]
    public class PlayerMovement : MonoBehaviour
    {

        [SerializeField] private float _moveSpeed = 2;

        public float CurrentMoveSpeed => IsMoving ? _moveSpeed : 0;

        private Rigidbody2D rb;
        [SerializeField] public Vector2 direction;

        public Animator animator; // TODO Remove null check

        public bool IsMoving { get; private set; }

        [SerializeField] private bool _enabled = true;

        void Awake()
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
        }

        void Start()
        {
            InputManager.Instance.KeyUp += () => direction.y = 1;
            InputManager.Instance.KeyDown += () => direction.y = -1;
            InputManager.Instance.KeyLeft += () => direction.x = -1;
            InputManager.Instance.KeyRight += () => direction.x = +1;

            if (SceneLoader.Instance != null)
            {
                SceneLoader.Instance.OverWorldSceneLoaded += () => _enabled = true;
                SceneLoader.Instance.OverWorldSceneUnloaded += () => _enabled = false;

                SceneLoader.Instance.CombatSceneLoaded += () => GetComponent<SpriteRenderer>().enabled = false;
                SceneLoader.Instance.CombatSceneUnloaded += () => GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                Debug.LogWarning("No Scene loader active. Skipped subscription");
            }
        }


        void FixedUpdate()
        {
            if (_enabled && (direction.x != 0 || direction.y != 0))
            {
                if (direction.x < 0)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }

                if (direction.x > 0)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }

                var newPos = rb.position + direction.normalized * _moveSpeed * Time.fixedDeltaTime;
                IsMoving = newPos != rb.position;
                //animator?.SetBool("isWalking", true);
                rb.MovePosition(newPos);
                //playerStatus.SetPosition(transform.localPosition);
            }
            else
            {
                IsMoving = false;
                //animator?.SetBool("isWalking", false);
            }

            direction = Vector2.zero;
        }
    }
}