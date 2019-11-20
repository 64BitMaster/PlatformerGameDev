using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform firePoint;
    public Transform firePoint45;
    public GameObject bolt;
    public PlayerController pc;
    public static float speed = 20f;
    public float velX = speed;
    private float velY = 0f;
    Rigidbody2D rb;
    private int frames;
    bool shoot;
    void Start()
    {
        frames = 0;
        shoot = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (frames > 20)
        {
            frames = 0;
        }
        if (Input.GetMouseButtonDown(0))
        {
            shoot = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            shoot = false;
        }
        if (shoot)
        {
            if(frames == 0)
            {
                Shoot();
            }
        }
        frames++;
        

    }
    void Shoot()
    {
        if (pc.isAngle)
            Instantiate(bolt, firePoint45.position, firePoint45.rotation);
            
        else
            Instantiate(bolt,firePoint.position,firePoint.rotation);
    }
}
