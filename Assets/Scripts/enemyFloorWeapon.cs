using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyFloorWeapon : MonoBehaviour
{
    public GameObject bolt;
    public Transform shootSpot;
    public Patrol patrol;
    private int frames;
    // Start is called before the first frame update
    void Start()
    {
        frames = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (frames >= 175)
        {
            frames = 0;
        }
        
        if (frames == 60 && patrol.shooting)
        {
            Shoot();
        }
        frames++;
    }
    void Shoot()
    {
        Instantiate(bolt, shootSpot.position, shootSpot.rotation);
    }
}
