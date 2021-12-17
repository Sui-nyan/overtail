using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Overtail;

namespace Overtail.Camera
{
    /// <summary>
    /// Class to smoothly follow a certain target continously.
    /// Can be assigned in Unity Inspector. (Defaults to <see cref="GameObject"/> with "Player" tag)
    /// <para /><see cref="currentTarget"/> target to be followed
    /// <para /><see cref="maxOffset"/> maximum distance before camera starts following
    /// <para /><see cref="smoothTime"/> camera move speed 
    /// </summary>
    public class CameraFollow : Camera
    {
        private Vector3 velocity = Vector3.zero;            // reference vector for SmoothDamp()

        [SerializeField] protected GameObject currentTarget;         // target to follow
        [SerializeField] protected float maxOffset;           // offset, zero => character will always be centered
        [SerializeField] protected float smoothTime = 0.25f;   // time it takes camera to reach target position. Zero is instant refocus


        protected void Start()
        {
            if (currentTarget == null)
            {
                currentTarget = FindPlayerObject();
                Debug.Log("No camera target found. Defaulted to <Player>" + currentTarget);
            }
        }

        void LateUpdate()
        {
            Follow();

        }

        public void SetTarget(GameObject newTarget)
        {
            currentTarget = newTarget;
        }

        public Vector2 TargetVector()
        {
            return TargetVector(currentTarget);
        }
        public Vector2 TargetVector(GameObject myTarget)
        {
            return myTarget.transform.position - transform.position;
        }
        public float Distance(GameObject toTarget)
        {
            return TargetVector(toTarget).magnitude;
        }

        public float Distance()
        {
            return TargetVector().magnitude;
        }
        public void Follow(GameObject targetObj, float cameraSpeed)
        {
            if (Distance() < maxOffset) return;
            Vector3 _targetPos = targetObj.transform.position;
            Vector3 _myPos = transform.position;

            Vector3 destination = new Vector3(_targetPos.x, _targetPos.y, _myPos.z);
            SetCamera(Vector3.SmoothDamp(Position3D, destination, ref velocity, cameraSpeed));
        }
        public void Follow(GameObject targetObj)
        {
            Follow(targetObj, smoothTime);
        }

        public void Follow()
        {
            Follow(currentTarget);
        }

        public GameObject FindPlayerObject()
        {
            Debug.Log($"CameraFollow> {GameObject.FindGameObjectWithTag("Player")}");
            return GameObject.FindGameObjectWithTag("Player");
            // Player p = FindObjectOfType(typeof(Player)) as Player;
            // return p.gameObject;
        }
    }
}