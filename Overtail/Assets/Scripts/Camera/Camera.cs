using Overtail.Camera;
using System.Collections;
using UnityEngine;

namespace Overtail.Camera
{
    /// <summary>
    /// Basic script to attach to camera. Derive specific camera behaviour from this
    /// </summary>
    public abstract class Camera : MonoBehaviour
    {
        /// <summary>
        /// Property to
        /// 1. get camera position in 2D plane
        /// 2. set camera position with <c>Vector2</c> without affecting z coordinate
        /// </summary>
        public Vector2 Position2D
        {
            get => transform.position;
            protected set => transform.position = new Vector3(value.x, value.y, transform.position.z);
        }

        /// <summary>
        /// Property to get and set camera position
        /// </summary>
        public Vector3 Position3D
        {
            get => transform.position;
            protected set => transform.position = value;
        }

        /// <summary>
        /// Method to set camera position to specific <c>Vector3</c> position
        /// </summary>
        /// <param name="v">New position as <c>Vector3</c></param>
        public void SetCamera(Vector3 v)
        {
            gameObject.transform.position = v;
        }

        /// <summary>
        /// Method to set camera position to specific <c>Vector2</c> position.
        /// Not affecting z position
        /// </summary>
        /// <param name="v">New position as <c>Vector2</c></param>
        public void SetCamera(Vector2 v)
        {
            transform.position = new Vector3(v.x, v.y, transform.position.z);
        }

        /// <summary>
        /// Moves camera relative to current position
        /// </summary>
        /// <param name="v">direction to move the </param>
        public void MoveCamera(Vector2 v)
        {
            transform.position += new Vector3(v.x, v.y);
        }

        /// <summary>
        /// Moves camera relative to current position.
        /// </summary>
        /// <param name="v"></param>
        public void MoveCamera(Vector3 v)
        {
            transform.position += v;
        }
    }
}