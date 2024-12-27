using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringController : MonoBehaviour
{
    [SerializeField] float jumpSpeed;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            var jumpingController = collider.gameObject.GetComponent<Player>();
            jumpingController.Jump(jumpSpeed);
        }
    }
}
