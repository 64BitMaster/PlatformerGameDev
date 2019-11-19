using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLedgeGrab : MonoBehaviour
{
    private float previousWallSpeed;
    public PlayerController pc;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ledgeGrab"))
        {
            pc.isHanging = true;
            other.attachedRigidbody.position = this.transform.position;
            previousWallSpeed = pc.wallSlideSpeed;
            pc.wallSlideSpeed = 0;
            other.attachedRigidbody.gravityScale = 0;
        }
            
        
    }
   
    void OnTriggerExit2D(Collider2D collision)
    {
        collision.attachedRigidbody.gravityScale = 4;
        pc.wallSlideSpeed = previousWallSpeed;
        pc.isHanging = false;
    }
}
