using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyAI : MonoBehaviour
{
    private Seeker seeker;
    private Rigidbody2D rb;
    private int currentWaypoint = 0;
    private PlayerController target;

    public float updateRate = 2;
    public Path path;
    public float followDistance = 50;
    public float speed = 300;
    public ForceMode2D fMode;
    public float nextWaypointDistance = 3;

    [HideInInspector]
    public bool pathIsEnded = false;

    
    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        target = FindObjectOfType<PlayerController>();
        if (target == null)
        {
            Debug.LogError("No Target");
            return;
        }
        seeker.StartPath(transform.position, target.transform.position, OnPathComplete);
        StartCoroutine(UpdatePath());
    }

    public void OnPathComplete(Path p)
    {
        if (p.error) {
            Debug.Log(p.error);
        }

        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        } 
    }

    IEnumerator UpdatePath()
    {
        seeker.StartPath(transform.position, target.transform.position, OnPathComplete);
        yield return new WaitForSeconds(1 / updateRate);
        StartCoroutine(UpdatePath());
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            return;
        }

        if (path == null)
        {
            return;
        }

        float playerDist = Vector2.Distance(transform.position, target.transform.position);
        if (playerDist > followDistance)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            if (pathIsEnded)
            {
                return;
            }
            pathIsEnded = true;
            return;
        }

        pathIsEnded = false;
        Vector2 direction = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        direction *= speed * Time.fixedDeltaTime;
        rb.AddForce(direction, fMode);
        float dist = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (dist < nextWaypointDistance)
        {
            currentWaypoint++;
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name.Contains("Bolt"))
        {
            Destroy(gameObject);
        }
    }
}
