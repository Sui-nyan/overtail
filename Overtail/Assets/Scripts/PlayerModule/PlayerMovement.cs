using Overtail.GUI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Overtail.Dialogue;

namespace Overtail.PlayerModule
{
    [RequireComponent(typeof(Rigidbody2D))]
    [DisallowMultipleComponent]
    public class PlayerMovement : MonoBehaviour
    {
        public bool IsMoving { get; private set; }
        public float CurrentMoveSpeed => IsMoving ? moveSpeed : 0;
        [SerializeField] public Vector2 direction;

        [SerializeField] private bool inMenu, inDialogue, inCombat;
        [SerializeField] private float moveSpeed = 32;
        private Rigidbody2D _rb;

        void Awake()
        {
            _rb = gameObject.GetComponent<Rigidbody2D>();
        }

        void Start()
        {
            InputManager.Instance.KeyUp += () => direction.y = 1;
            InputManager.Instance.KeyDown += () => direction.y = -1;
            InputManager.Instance.KeyLeft += () => direction.x = -1;
            InputManager.Instance.KeyRight += () => direction.x = +1;
        }

        void FixedUpdate()
        {
            inMenu = FindObjectOfType<MenuManager>()?.MenuIsActive ?? false;
            inDialogue = FindObjectOfType<DialogueManager>()?.IsOpen ?? false;
            inCombat = SceneManager.GetActiveScene().name.Contains("Combat");

            if (!(inMenu || inDialogue || inCombat) && (direction.x != 0 || direction.y != 0))
            {

                if (direction.x < 0)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }

                if (direction.x > 0)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }

                var oldPos = _rb.position;
                var newPos = oldPos + direction.normalized * moveSpeed * Time.fixedDeltaTime;
                IsMoving = newPos != oldPos;
                _rb.MovePosition(newPos);
            }
            else
            {
                IsMoving = false;
            }

            direction = Vector2.zero;
        }
    }
}
