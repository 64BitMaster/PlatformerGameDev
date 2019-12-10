using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofClinger : MonoBehaviour
{
    private PlayerController pc;
    private int frames = 0;
    private Animator anim;
    public Transform firePoint;
    public GameObject bolt;
    public float fireRange = 10;
    public float fireDelay = 30;
    public int HP = 3;
    private bool dead = false;
    private bool shoot = false;
    // Start is called before the first frame update
    void Start()
    {
        // Locate Player
        pc = FindObjectOfType<PlayerController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (frames > fireDelay)
        {
            frames = 0;
        }

        // Sense
        bool inRange = Mathf.Abs(transform.position.x - pc.transform.position.x) < fireRange;
        bool playerBelow = pc.transform.position.y < transform.position.y;

        // Act
        if (inRange && playerBelow)
        {
            shoot = true;
        }
        else
        {
            shoot = false;
        }
        frames++;
        UpdateAnimations();
        CheckDeath();
    }
    void Shoot()
    {
        // Fire downward
        var rotation = Quaternion.Euler(new Vector3(0, 0, -90));
        Instantiate(bolt, firePoint.position, rotation);
    }

    private void UpdateAnimations()
    {
        anim.SetBool("shoot", shoot);
        anim.SetBool("dead", dead);
    }

    private void CheckDeath()
    {
        if (HP <= 0)
        {
            dead = true;
        }
    }
    private void DestroyRoofEnemy()
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
