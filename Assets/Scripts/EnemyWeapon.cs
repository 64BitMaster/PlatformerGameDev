using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
      
    private PlayerController pc;
    private int frames;
    private bool shoot;

    public GameObject bolt;
    public float shootDistance = 10;
    public int delayOfFire = 20;

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

        float playerDist = Vector2.Distance(transform.position, pc.transform.position);
        if (playerDist < shootDistance)
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
        if (transform.position.x > pc.transform.position.x)
        {
            var rotation = Quaternion.Euler(new Vector3(0, 0, 180));
            Instantiate(bolt, new Vector2(transform.position.x - 2.0f, transform.position.y), rotation);
        }
        else
        {
            var rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            Instantiate(bolt, new Vector2(transform.position.x + 2.0f, transform.position.y), rotation);
            
        }
    }
}