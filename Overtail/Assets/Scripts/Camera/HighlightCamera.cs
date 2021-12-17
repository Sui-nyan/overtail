using UnityEngine;
using System.Collections.Generic;
using System.Collections;
namespace Overtail.Camera
{
    /// <summary>
    /// Camera to follow a queued up list of targets.
    /// </summary>
    public class HighlightCamera : CameraFollow
    {
        [SerializeField] private LinkedList<TrackingJob> queue = new LinkedList<TrackingJob>();

        // negative duration => permanent
        [SerializeField] protected GameObject currentTarget;
        [SerializeField] protected float currentDuration;

        [SerializeField] protected float elapsedTime;

        [Header("Debug")]
        [SerializeField] private int debugQueueCount = 0;
        [SerializeField] private float quickPeekTime = .1f;

        protected override void LateUpdate()
        {
            debugQueueCount = queue.Count;

            if (!_inputCoroutine()) return;

            // Duration is over or no target currently
            if (currentTarget == null || (currentDuration >= 0 && elapsedTime > currentDuration))
            {
                if (!GoToNext()) // Anything in queue?
                {
                    if (DefaultTarget != null) // Fallback value?
                    {
                        SmoothFocus(DefaultTarget);
                    }
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
        /// Go-to method.
        /// Peeks at some Object for a certain time.
        /// Overrides current if it's being tracked indefinitely.
        /// Jumps queue if not.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="duration"></param>
        public void Peek(GameObject target, float duration)
        {
            if (currentDuration == -1)
            {
                Follow(target, duration);
            }
            else
            {
                JumpQueue(target, duration);
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
        public (GameObject, float) Dequeue()
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
            if (queue.Count == 0) return false;
            (GameObject newTarget, float newDuration) = Dequeue();

            // try again if t was empty
            if (newTarget == null) return GoToNext();

            Follow(newTarget, newDuration);
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
        /// <see cref="Follow"/> but puts the currently tracked target back onto the queue with leftover time
        /// </summary>
        /// <param name="target"></param>
        /// <param name="duration"></param>
        private void JumpQueue(GameObject target, float duration)
        {
            TrackingJob current = new TrackingJob(
                currentTarget,
                currentDuration < 0 ? -1 : Mathf.Max(currentDuration - elapsedTime, 0));

            Follow(target, duration);
            queue.AddFirst(current);
        }

        /// <summary>
        /// Follow a target GameObject for a certain duration. Overrides current target.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="duration"></param>
        private void Follow(GameObject target, float duration)
        {
            currentDuration = duration;
            currentTarget = target;
            elapsedTime = 0;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="target"></param>
        private new void Follow(GameObject target)
        {
            Follow(target, -1);
        }



        private bool _inputCoroutine()
        {
            // returns true if calling context should continue;
            // Test code for funsies

            if (Input.GetKeyDown(KeyCode.Tab)) GoToNext();

            if (Input.GetKey(KeyCode.Space))
            {
                Focus(GameObject.FindGameObjectWithTag("Player"));
                return false;
            }

            if (Input.GetKeyDown(KeyCode.Alpha1)) Peek(GameObject.FindGameObjectWithTag("Player"), -1);
            if (Input.GetKeyDown(KeyCode.Alpha2)) Peek(GameObject.FindGameObjectWithTag("Dummy1"), -1);
            if (Input.GetKeyDown(KeyCode.Alpha3)) Peek(GameObject.FindGameObjectWithTag("Dummy2"), -1);
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                Enqueue(GameObject.FindGameObjectWithTag("Player"), 1f);
                Enqueue(GameObject.FindGameObjectWithTag("Dummy1"), 1f);
                Enqueue(GameObject.FindGameObjectWithTag("Dummy2"), 1f);
                Enqueue(GameObject.FindGameObjectWithTag("Player"), 1f);
                Enqueue(GameObject.FindGameObjectWithTag("Dummy1"), 1f);
                Enqueue(GameObject.FindGameObjectWithTag("Dummy2"), 1f);
                Enqueue(GameObject.FindGameObjectWithTag("Player"), 1f);
                Enqueue(GameObject.FindGameObjectWithTag("Dummy1"), 1f);
                Enqueue(GameObject.FindGameObjectWithTag("Dummy2"), 1f);
                GoToNext();
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))Peek(GameObject.FindGameObjectWithTag("Dummy1"), .2f);
            if (Input.GetKeyDown(KeyCode.Alpha5)) Peek(GameObject.FindGameObjectWithTag("Dummy2"), .2f);

            return true;
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