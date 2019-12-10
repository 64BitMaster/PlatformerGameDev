using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofEnemyWeapon : MonoBehaviour
{

    private PlayerController pc;
    private int frames;
    private bool shoot;

    public GameObject bolt;
    public float xDistanceTrigger = 5;
    public int delayOfFire = 20;
    public int HP = 3;
    private bool dead = false;
    // Start is called before the first frame update 
    void Start()
    {
        pc = FindObjectOfType<PlayerController>();
        frames = 0;
        shoot = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (frames > delayOfFire)
        {
            frames = 0;
        }

        bool inRange = Mathf.Abs(transform.position.x - pc.transform.position.x) < xDistanceTrigger;
        bool playerBelow = pc.transform.position.y < transform.position.y;
        if (inRange && playerBelow)
        {
            shoot = true;
        }
        else
        {
            shoot = false;
        }

        if (shoot)
        {
            if (frames == 0)
            {
                Shoot();
            }
        }
        frames++;
    }
    void Shoot()
    {
            var rotation = Quaternion.Euler(new Vector3(0, 0, -90));
            Instantiate(bolt, new Vector2(transform.position.x, transform.position.y - 5.0f), rotation);    
    }
}