using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Overtail;

namespace Overtail.Camera
{
    public class CameraFollow : Camera
    {
        [SerializeField] private GameObject target;
        [SerializeField] private float maxOffset;
        [SerializeField] public float smoothTime = 0.25f;
        private Vector3 velocity = Vector3.zero;
        public Vector2 TargetVector => target.transform.position - transform.position;
        public float Distance => TargetVector.magnitude;

        void Start()
        {
            base.Start();
            if (target == null)
            {
                target = FindPlayerObject();
                Debug.Log("No camera target found. Default to <Player>" + target);
            }
        }

        void LateUpdate()
        {
            if (Distance > maxOffset)
            {
                Follow();
            }
        }

        public void Follow(GameObject targetObj)
        {
            Vector3 _targetPos = targetObj.transform.position;
            Vector3 _myPos = transform.position;

            Vector3 destination = new Vector3(_targetPos.x, _targetPos.y, _myPos.z);
            SetCamera(Vector3.SmoothDamp(Position3D, destination, ref velocity, smoothTime));
        }

        public void Follow()
        {
            Follow(target);
        }

        public GameObject FindPlayerObject()
        {
            Player p = FindObjectOfType(typeof(Player)) as Player;
            return p.gameObject;
        }
    }
}