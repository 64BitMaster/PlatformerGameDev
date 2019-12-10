using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class Rocket : MonoBehaviour
{
    private Seeker seeker;
    private Rigidbody2D rb;
    private int currentWaypoint = 0;
    private PlayerController target;
    private Vector3 targetLocation;
    private bool facingRight = false;
    private float updateRate = 2;
    private Path path;
    private float nextWaypointDistance = 0.5f;

    public int damage = 30;
    public float speed = 300;
    public ForceMode2D fMode;


    [HideInInspector]
    public bool pathIsEnded = false;


    // Start is called before the first frame update
    void Start()
    {
        // Get Seeker component (For PathFinding)
        seeker = GetComponent<Seeker>();

        // Get RigidBody to apply movement to
        rb = GetComponent<Rigidbody2D>();

        // Find Player Object to set as target
        target = FindObjectOfType<PlayerController>();
        if (target == null)
        {
            Debug.LogError("No Target");
            return;
        }

        targetLocation = target.transform.position;
        // Begin path creation
        seeker.StartPath(transform.position, target.transform.position, OnPathComplete);

        // Keep updating path as player moves
        StartCoroutine(UpdatePath());
    }

    public void OnPathComplete(Path p)
    {
        if (p.error)
        {
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
        // Update path
        seeker.StartPath(transform.position, target.transform.position, OnPathComplete);
        yield return new WaitForSeconds(1 / updateRate);

        // Loop back to update path again
        StartCoroutine(UpdatePath());
    }

    void FixedUpdate()
    {
        // No Target end early
        if (target == null)
        {
            return;
        }

        // No Path end early
        if (path == null)
        {
            return;
        }

        // Check if pathing is done
        if (currentWaypoint >= path.vectorPath.Count)
        {
            if (pathIsEnded)
            {
                Destroy(gameObject);
            }
            pathIsEnded = true;
            
            return;
        }
        pathIsEnded = false;

        // Calcualte direction and magnitude to move
        Vector2 direction = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        direction *= speed * Time.fixedDeltaTime;

        // Apply force to cause movement
        rb.AddForce(direction, fMode);

        // Check if needed to set destination to next waypoint
        float dist = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (dist < nextWaypointDistance)
        {
            currentWaypoint++;
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name.Contains("Player")) // If collide with player apply damage
        {
            var health = col.gameObject.GetComponent<HealthController>();
            
            health?.applyDamage(damage);
        }

        Destroy(gameObject);
    }
}

