using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameController : MonoBehaviour
{

    public Animator animator;


    private void OnTriggerEnter2D(Collider2D col) {
        animator.SetTrigger("FadeOut");
        Debug.Log("ENDING TRIGGERED!");

    }

}
