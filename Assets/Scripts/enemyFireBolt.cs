using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyFireBolt : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 10;
    private float velX;
    private float velY;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        velX = speed;
        if (PlayerAngle.getAngle())
            velY = speed;
        else
            velY = 0f;


        rb.velocity = transform.right * velX + transform.up * velY;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name.Contains("Player"))
        {
            var health = col.gameObject.GetComponent<HealthController>();

            health?.applyDamage(damage);
        }

        Destroy(gameObject);
    }
}
