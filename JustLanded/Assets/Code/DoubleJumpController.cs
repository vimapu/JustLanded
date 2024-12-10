using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpController : MonoBehaviour
{
    private Jumping jumpingController;

    void Start()
    {
        jumpingController = GameObject.FindGameObjectWithTag("Player").GetComponent<Jumping>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            jumpingController.LearnDoubleJump();
            Destroy(gameObject);
        }

    }

}
