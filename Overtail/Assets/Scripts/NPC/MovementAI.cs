using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Pathfinding; //namespace imported from https://arongranberg.com/astar/front

/*
 * Movment behavior, that follows around a target
 */
public class MovementAI : MonoBehaviour
{
    public Transform target;

    public float moveSpeed = 5f;
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWayPoint = 0;
    bool reachEndofPath = false;

    Seeker seeker; //External Script imported from https://arongranberg.com/astar/front
    Rigidbody2D rb;
    
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        seeker.StartPath(rb.position, target.position, OnPathComplete);
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (path == null)
            return;

        if (currentWayPoint >= path.vectorPath.Count)
        {
            reachEndofPath = true;
            return;
        }
        else
        {
            reachEndofPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;

        Vector2 force = direction * moveSpeed * Time.fixedDeltaTime;
        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);
        if (distance < nextWaypointDistance)
        {
            currentWayPoint++;
        }

    }

}
