using UnityEngine;
using System.Collections.Generic;
using System.Collections;
namespace Overtail.Camera
{
    /// <summary>
    /// Camera to follow a queued up list of targets.
    /// </summary>
    public class HighlightCamera : BasicCamera
    {
        [SerializeField] private LinkedList<TrackingJob> queue = new LinkedList<TrackingJob>();

        // negative duration => permanent
        [SerializeField] protected GameObject currentTarget;
        [SerializeField] protected float currentDuration;

        [SerializeField] protected float elapsedTime;

        [Header("Debug")]
        [SerializeField] private int debugQueueCount = 0;
        [SerializeField] private float quickPeekTime = .1f;

        public int QueueCount => queue.Count;

        protected override void LateUpdate()
        {
            debugQueueCount = queue.Count;

            // if Duration is over or no target currently
            if (currentTarget == null || (currentDuration >= 0 && elapsedTime > currentDuration))
            {
                if (!GoToNext()) // Anything in queue?
                {
                    // Fallback value
                    base.LateUpdate();
                }
            }
            else
            {
                if (currentDuration == -1)
                    SmoothFocus(currentTarget);
                else
                    SmoothFocus(currentTarget, Mathf.Clamp(currentDuration, quickPeekTime, DefaultTime));
                if (currentDuration >= 0) elapsedTime += Time.deltaTime;
            }
        }

        /// <summary>
        /// Add a target with duration to queue.
        /// </summary>
        /// <param name="newTarget"></param>
        /// <param name="newDuration"></param>
        public void Enqueue(GameObject newTarget, float newDuration)
        {
            queue.AddLast(new TrackingJob(newTarget, newDuration));
        }

        /// <summary>
        /// Get next element (or tupel) from queue
        /// </summary>
        /// <returns></returns>
        private (GameObject, float) Dequeue()
        {
            TrackingJob t = queue.First.Value;
            queue.RemoveFirst();

            return (t.target, t.duration);
        }

        /// <summary>
        /// Sets target to next in Queue<para/>
        /// Returns true if successful (and sets new target)<para/>
        /// Returns false if queue is empty (target not changed)
        /// </summary>
        /// <returns></returns>
        public bool GoToNext()
        {
            if (queue.Count == 0) ClearAll();
            (GameObject newTarget, float newDuration) = Dequeue();

            // try again if t was empty
            if (newTarget == null) return GoToNext();

            SetTarget(newTarget, newDuration);
            return true;
        }

        /// <summary>
        /// Clear Queue and remove currently tracked target
        /// </summary>
        public void ClearAll()
        {
            queue.Clear();
            currentTarget = null;
            currentDuration = 0;
            elapsedTime = 0;
        }

        /// <summary>
        /// <see cref="SetTarget"/> but puts the currently tracked target back onto the queue with leftover time
        /// </summary>
        /// <param name="target"></param>
        /// <param name="duration"></param>
        public void JumpQueue(GameObject target, float duration)
        {
            TrackingJob current = new TrackingJob(
                currentTarget,
                currentDuration < 0 ? -1 : Mathf.Max(currentDuration - elapsedTime, 0));

            SetTarget(target, duration);
            queue.AddFirst(current);
        }

        /// <summary>
        /// Follow a target GameObject for a certain duration. Overrides current target.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="duration"></param>
        public void SetTarget(GameObject target, float duration = -1)
        {
            currentDuration = duration;
            currentTarget = target;
            elapsedTime = 0;
        }
    }
    internal class TrackingJob
    {
        public readonly GameObject target;
        public readonly float duration;

        public TrackingJob(GameObject target, float duration)
        {
            this.target = target;
            this.duration = duration;
        }
    }
}