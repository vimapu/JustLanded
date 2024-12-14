using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblePistolController : MonoBehaviour
{

    private MovementController movementController;

    void Start()
    {
        movementController = GameObject.FindGameObjectWithTag("Player").GetComponent<MovementController>();
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            movementController.CollectPistol();
        }
        Destroy(gameObject);
    }
}
