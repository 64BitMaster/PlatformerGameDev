using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    //public FireBolt fireBolt;
    
    private bool movingRight = true;
    public bool canShoot;
    //public bool foundPlayer = false;
    private PlayerController target;
    private float speedOld;
    private Animator anim;
    private int frames = 0;

    public int HP = 3;

    public float speed;
    public float dropDistance;
    public bool shooting;
    private bool dead = false;
    public Transform groundDetection;
    
    public Transform firePoint;
    public GameObject bolt;
    public float fireRange = 10;
    public float fireDelay = 30;
    public LayerMask whatIsPlayer;

    private void Start()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
        speedOld = speed;
        anim = GetComponent<Animator>();
        target = FindObjectOfType<PlayerController>();
    }
    // Update is called once per frame
    private void Update()
    {
        UpdateAnimations();
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        RaycastHit2D ground = Physics2D.Raycast(groundDetection.position, Vector2.down, dropDistance);
        if (ground.collider == false)
        {
            if (movingRight == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }
        }

        WeaponFireCheck();
        CheckDeath();
        
    }

    private void WeaponFireCheck()
    {
        // Delay in fire
        if (frames > fireDelay)
        {
            frames = 0;
        }

        // If player is within range to fire
        bool targetIsRight = transform.position.x < target.transform.position.x;
        bool facingTarget = targetIsRight == movingRight;

        float playerDist = Mathf.Abs(transform.position.x - target.transform.position.x);

        if (facingTarget && playerDist < fireRange)
        {
            speed = 0;
            shooting = true;

            if (frames == 0)
            {
                //Shoot();
            }
        }
        else
        {
            speed = speedOld;
            shooting = false;
            frames = 1;
        }
        frames++;
    }

    private void Shoot()
    {
        bool targetIsRight = transform.position.x < target.transform.position.x;
        var rotation = targetIsRight ?
            Quaternion.Euler(new Vector3(0, 0, 0)) : // Fire Right
            Quaternion.Euler(new Vector3(0, 0, 180)); // Fire Left

        Instantiate(bolt, firePoint.position, rotation); // Create projectile
    }
    private void shotPlayer(bool foundPlayer)
    {
        if (foundPlayer)
        {
            shooting = true;
            canShoot = true;
        }
        else
        {
            shooting = false;
            canShoot = false;
        }
        
        
    }
    private void UpdateAnimations()
    {
        anim.SetBool("shooting",shooting);
        anim.SetBool("dead", dead);
    }

    private void CheckDeath()
    {
        if (HP <= 0)
        {
            dead = true;
        }
    }
    private void DestroyGroundEnemy()
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