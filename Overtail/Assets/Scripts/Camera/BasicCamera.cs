using UnityEngine;

/// <summary>
/// Please just use <see cref="Overtail.Camera.PlayerCamera"/>
/// </summary>
namespace Overtail.Camera
{
    /// <summary>
    /// Class to smoothly follow a certain target continously.
    /// Can be assigned in Unity Inspector. (Defaults to <see cref="GameObject"/> with "Player" tag)
    /// Please use <see cref="Overtail.Camera.PlayerCamera"/>
    /// <para /><see cref="DefaultTarget"/> target to be followed
    /// <para /><see cref="DefaultOffset"/> maximum distance before camera starts following
    /// <para /><see cref="DefaultTime"/> camera move speed 
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class BasicCamera : MonoBehaviour
    {
        // reference vector for SmoothDamp()
        private Vector3 velocity = Vector3.zero;

        // target to keep following
        [SerializeField] private GameObject defaultTarget;
        // offset to target, zero will center target
        [SerializeField] private float defaultOffset = 0f;
        // time it takes camera to reach target position. Zero is instant
        [SerializeField] private float defaultTime = 0.3f;

        protected GameObject DefaultTarget { get => defaultTarget; set => defaultTarget = value; }
        protected float DefaultOffset { get => defaultOffset; set => defaultOffset = value; }
        protected float DefaultTime { get => defaultTime; set => defaultTime = value; }

        protected virtual void LateUpdate()
        {
            if (DefaultTarget == null) return;

            SmoothFocus();
        }

        /// <summary>
        /// Distance from camera object to some <see cref="GameObject"/>
        /// </summary>
        /// <returns>Distance to target</returns>
        protected float Distance(GameObject target)
        {
            Vector2 cameraPosition = gameObject.transform.position;
            Vector2 targetPosition = target.gameObject.transform.position;
            return (targetPosition - cameraPosition).magnitude;
        }

        /// <summary>
        /// Instantly centers camera around target.
        /// </summary>
        /// <param name="target"></param>
        public void SnapTo(GameObject target)
        {
            SmoothFocus(target, 0, 0);
        }

        /// <summary>
        /// Set which GameObject to follow
        /// </summary>
        /// <param name="target"></param>
        public virtual void SetTarget(GameObject target)
        {
            DefaultTarget = target;
        }

        /// <summary>
        /// Moves camera towards target (partly).<para/>
        /// Call in <see cref="LateUpdate"/> to smoothly follow target.
        /// </summary>
        /// <param name="target">Target GameObject</param>
        /// <param name="time">Time to reach target</param>
        /// <param name="offset">Radius around target where this is ineffective</param>
        protected void SmoothFocus(GameObject target = null, float time = -1, float offset = -1)
        {
            if (target == null) target = DefaultTarget;
            if (time < 0) time = DefaultTime;
            if (offset < 0) offset = DefaultOffset;

            if (Distance(target) < offset) return;

            Vector3 cameraPosition = gameObject.transform.position;
            Vector3 targetPosition = target.gameObject.transform.position;
            Vector3 finalPosition = new Vector3(targetPosition.x, targetPosition.y, cameraPosition.z);

            gameObject.transform.position = Vector3.SmoothDamp(cameraPosition, finalPosition, ref velocity, time);
        }
    }
}
