using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class Flyer : MonoBehaviour
{
    private Seeker seeker;
    private Rigidbody2D rb;
    private int currentWaypoint = 0;
    private PlayerController target;
    private bool facingRight = false;
    private float updateRate = 2;
    private Path path;
    private float nextWaypointDistance = 3;
    private GameObject rocket;
    public bool RocketFired;
    private Animator anim;
    public int HP = 3;
    private bool dead = false;
    public Transform firePoint;
    public GameObject bolt;
    public float fireRange = 10;
    public float aggroDistance = 50;
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
        anim = GetComponent<Animator>();
        // Find Player Object to set as target
        target = FindObjectOfType<PlayerController>();
        if (target == null)
        {
            Debug.LogError("No Target");
            return;
        }

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
    private void Update()
    {
        CheckDeath();
        UpdateAnimations();
    }
    void FixedUpdate()
    {
        // No target end early
        if (target == null)
        {
            return;
        }

        // No Path end early
        if (path == null)
        {
            return;
        }

        // Calculate player location parameters
        float playerDist = Vector2.Distance(transform.position, target.transform.position);
        float playerxDist = Mathf.Abs(transform.position.x - target.transform.position.x);
        bool playerIsRight = target.transform.position.x > transform.position.x;

        // Adjust facing
        if (playerIsRight != facingRight)
        {
            facingRight = !facingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }

        // Act weapon
        WeaponFireCheck();

        // Act if player is close
        if (playerDist < aggroDistance)
        {
            if (currentWaypoint >= path.vectorPath.Count)
            {
                if (pathIsEnded)
                {
                    // Do Something when reaching destination
                    return;
                }
                pathIsEnded = true;
                return;
            }
            pathIsEnded = false;

            // Calcualte direction and magnitude to move
            Vector2 direction = (path.vectorPath[currentWaypoint] - transform.position).normalized;
            direction *= speed * Time.fixedDeltaTime;

            if (playerxDist < fireRange) // Stop moving on x if within range
                direction.x = 0;

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
    }
    private void WeaponFireCheck()
    {
        RocketFired = (rocket != null);

        // If player is within range to fire
        float playerDist = Vector2.Distance(transform.position, target.transform.position);
        if (playerDist < fireRange)
        {
            if (!RocketFired)
            {
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        bool targetIsRight = transform.position.x < target.transform.position.x;
        var rotation = targetIsRight ?
            Quaternion.Euler(new Vector3(0, 0, 0)) : // Fire Right
            Quaternion.Euler(new Vector3(0, 0, 180)); // Fire Left
        if (targetIsRight)
            rocket = Instantiate(bolt, firePoint.position, rotation); // Create projectile
        else
        {
            rocket = Instantiate(bolt, firePoint.position, rotation);
            rocket.transform.Rotate(0f,180f,0f);
        }
            
    }

    private void UpdateAnimations()
    {
        anim.SetBool("RocketFired", RocketFired);
        anim.SetBool("dead", dead);
    }

    private void CheckDeath()
    {
        if (HP <= 0)
        {
            dead = true;
        }
    }
    private void DestroyFlyerEnemy()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name.Contains("Bolt") && !col.name.Contains("enemy")) // Only destory if hit by a non-enemy
        {
            HP--;
        }
    }
}
