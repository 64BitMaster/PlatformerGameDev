using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBolt : MonoBehaviour
{
    public float speed = 20f;
    private float velX;
    private float velY;
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        velX = speed;
        if (PlayerAngle.getAngle())
            velY = speed;
        else
            velY = 0f;


        rb.velocity = transform.right*velX + transform.up*velY;
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(gameObject);
    }
}
