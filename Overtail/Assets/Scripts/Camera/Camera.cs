using Overtail.Camera;
using System.Collections;
using UnityEngine;

namespace Overtail.Camera
{
    public abstract class Camera : MonoBehaviour
    {
        // TODO Fix camera position in case it was reset
        protected void Start()
        {
            if (transform.position.z >= 0)
            {
                Vector3 v = transform.position;
                transform.position = new Vector3(v.x, v.y, -10);
            }
        }
        public Vector2 Position2D
        {
            get => transform.position;
            protected set => transform.position = new Vector3(value.x, value.y, transform.position.z);
        }

        public Vector3 Position3D
        {
            get => transform.position;
            protected set => transform.position = value;
        }

        public void SetCamera(Vector3 v)
        {
            gameObject.transform.position = v;
        }

        public void SetCamera(Vector2 v)
        {
            transform.position = new Vector3(v.x, v.y, transform.position.z);
        }

        public void MoveCamera(Vector2 v)
        {
            transform.position += new Vector3(v.x, v.y);
        }

        public void MoveCamera(Vector3 v)
        {
            transform.position += v;
        }
    }
}