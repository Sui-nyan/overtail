using UnityEngine;

namespace Overtail.Camera
{
    /// <summary>
    /// Default camera to follow player. Just attach this to main camera and either
    /// a) Assign player GameObject to target field or...
    /// b) Add "Player"-Tag to player GameObject
    /// </summary>
    public class PlayerCamera : BasicCamera
    {
        [Header("Cooldown")]
        [SerializeField] private float refocusCooldown = 5;
        [SerializeField] private float currentCooldown = 0;

        private void Start()
        {
            FindPlayerTarget();
        }

        protected override void LateUpdate()
        {
            ResetPlayerTarget();
            base.LateUpdate();
        }
        
        /// <summary>
        /// Finds as assigns GameObject with "Player" tag as tracking target.
        /// </summary>
        public void FindPlayerTarget()
        {
            DefaultTarget = GameObject.FindGameObjectWithTag("Player");
            if (DefaultTarget == null)
                Debug.Log("[Camera] setup failed. No GameObject with <Player> tag found.");
            else
                Debug.Log($"[Camera] Following <{DefaultTarget.name}>");
        }

        /// <summary>
        /// Use in <see cref="LateUpdate"/>.
        /// Calls <see cref="FindPlayerTarget"/> with a cooldown time.
        /// </summary>
        private void ResetPlayerTarget()
        {
            if (DefaultTarget.tag == "Player") return;

            if (currentCooldown > 0)
            {
                currentCooldown = Mathf.Max(currentCooldown - Time.deltaTime, 0);
                return;
            }

            Debug.Log("[Camera] No default target found.");
            FindPlayerTarget();
            currentCooldown = refocusCooldown;
        }
    }
}