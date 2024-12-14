using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleGearController : MonoBehaviour
{

    [SerializeField] float value = 1f;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            // TODO: add to the score
            Destroy(gameObject);
        }
    }

}
