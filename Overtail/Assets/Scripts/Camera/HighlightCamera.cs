using UnityEngine;
using System.Collections.Generic;
using System.Collections;
namespace Overtail.Camera
{
    /// <summary>
    /// Camera to follow 
    /// </summary>
    public class HighlightCamera : CameraFollow
    {
        [SerializeField] private LinkedList<TargetListing> targetQueue = new LinkedList<TargetListing>();

        // inherited
        // protected GameObject currentTarget;
        // protected float maxOffset;
        // protected float smoothTime;

        // negative duration => permanent
        [SerializeField] protected float currentDuration;
        [SerializeField] protected float elapsedTime;

        protected void Start()
        {
            base.Start();
            currentDuration = -1;
        }
        private void LateUpdate()
        {
            // Test code for funsies
            {
                if (Input.GetKeyDown(KeyCode.Alpha1)) SetTarget(GameObject.FindGameObjectWithTag("Player"));
                if (Input.GetKeyDown(KeyCode.Alpha2)) SetTarget(GameObject.FindGameObjectWithTag("Dummy1"));
                if (Input.GetKeyDown(KeyCode.Alpha3)) SetTarget(GameObject.FindGameObjectWithTag("Dummy2"));

                if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    QueueTarget(GameObject.FindGameObjectWithTag("Dummy1"), .1f);
                    QueueTarget(GameObject.FindGameObjectWithTag("Dummy2"), .1f);
                }

                if (Input.GetKeyDown(KeyCode.Tab)) GoNext();

                if (Input.GetKeyDown(KeyCode.Alpha5))
                {
                    JumpQueue(GameObject.FindGameObjectWithTag("Dummy1"), .1f);
                }
                if (Input.GetKey(KeyCode.Space))
                {
                    Debug.Log(GameObject.FindGameObjectWithTag("Player"));
                    Follow(GameObject.FindGameObjectWithTag("Player"));
                    return;
                }
            }

            if (currentTarget == null && targetQueue.Count == 0) return;

            if (currentDuration < 0 || elapsedTime < currentDuration)
            {
                elapsedTime += Time.deltaTime;
                Follow(currentTarget,
                    currentDuration < 0 ? smoothTime : Mathf.Min(smoothTime, 0.6f * currentDuration));
            }
            else
            {
                Clear();
                GoNext();
            }
        }

        private void Clear()
        {
            currentTarget = null;
            currentDuration = 0;
            elapsedTime = 0;
        }

        public void SetTarget(GameObject newTarget)
        {
            SetTarget(newTarget, -1);
        }
        public void SetTarget(GameObject newTarget, float duration)
        {
            this.currentTarget = newTarget;
            this.currentDuration = duration;
            elapsedTime = 0;
        }

        public void QueueTarget(GameObject newTarget, float newDuration)
        {
            targetQueue.AddLast(new TargetListing(newTarget, newDuration));
        }

        public void JumpQueue(GameObject newTarget, float newDuration)
        {
            TargetListing oldTarget = new TargetListing
                (this.currentTarget,
                this.currentDuration < 0 ? -1 : Mathf.Max(this.currentDuration - elapsedTime, 0));
            targetQueue.AddFirst(oldTarget);

            SetTarget(newTarget, newDuration);
        }

        private bool GoNext()
        {
            if (targetQueue.Count == 0) return false;

            TargetListing t = targetQueue.First.Value;
            targetQueue.RemoveFirst();

            if (t.target == null) return GoNext(); // try again if t was empty

            SetTarget(t.target, t.duration);
            return true;
        }

    }
    internal class TargetListing
    {
        public readonly GameObject target;
        public readonly float duration;

        public TargetListing(GameObject target, float duration)
        {
            this.target = target;
            this.duration = duration;
        }
    }
}