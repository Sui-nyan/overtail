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
        private bool sprinting = false;
        public float CurrentMoveSpeed => IsMoving ? baseSpeed : 0;
        [SerializeField] public Vector2 direction;

        [SerializeField] private bool inMenu, inDialogue, inCombat;
        [SerializeField] private float baseSpeed = 5;
        [SerializeField] private float sprintMultiplier = 1.5f;
        public float externalMultiplier = 1f;

        private Rigidbody2D _rb;

        void Awake()
        {
            _rb = gameObject.GetComponent<Rigidbody2D>();
        }

        void Start()
        {
            if (InputManager.Instance != null)
            {
                InputManager.Instance.KeyUp += () => direction.y = 1;
                InputManager.Instance.KeyDown += () => direction.y = -1;
                InputManager.Instance.KeyLeft += () => direction.x = -1;
                InputManager.Instance.KeyRight += () => direction.x = +1;
                InputManager.Instance.KeyConfirm += () => sprinting = !sprinting;
            }
            else
            {
                Debug.LogWarning("No InputManager found");
            }
        }

        void FixedUpdate()
        {
            inMenu = FindObjectOfType<MenuManager>()?.MenuIsActive ?? false;
            inDialogue = FindObjectOfType<DialogueManager>()?.IsOpen ?? false;
            inCombat = SceneManager.GetActiveScene().name.Contains("Combat");

            if (!(inMenu || inDialogue || inCombat) && (direction.x != 0 || direction.y != 0))
            {
                Move();
                IsMoving = true;
                direction = Vector2.zero;
            }
            else
            {
                IsMoving = false;
            }
        }

        public void Move()
        {
            var oldPos = _rb.position;
            var delta = direction.normalized * baseSpeed * Time.fixedDeltaTime * externalMultiplier;
            delta *= sprinting ? sprintMultiplier : 1;
            Vector2 newPos = oldPos + delta;
            _rb.MovePosition(newPos);
        }
    }
}
