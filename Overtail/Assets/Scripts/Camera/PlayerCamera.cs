using UnityEngine;

namespace Overtail.Camera
{
    public class PlayerCamera : CameraFollow
    {
        [Header("Cooldown")]
        [SerializeField] private float refocusCooldown = 5;
        [SerializeField] private float currentCooldown = 0;

        private void Start()
        {
            FollowPlayer();
        }

        protected override void LateUpdate()
        {
            RefollowPlayer();
            base.LateUpdate();
        }
        public void FollowPlayer()
        {
            DefaultTarget = GameObject.FindGameObjectWithTag("Player");
            if (DefaultTarget == null)
                Debug.Log("[Camera] setup failed. No GameObject with <Player> tag found.");
            else
                Debug.Log($"[Camera] Following <{DefaultTarget.name}>");
        }

        private void RefollowPlayer()
        {
            if (currentCooldown > 0)
            {
                currentCooldown = Mathf.Max(currentCooldown - Time.deltaTime, 0);
                return;
            }

            if (DefaultTarget.tag == "Player") return;

            Debug.Log("[Camera] No default target found.");
            FollowPlayer();
            currentCooldown = refocusCooldown;
        }
    }
}