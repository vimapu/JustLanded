using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblePistolController : MonoBehaviour
{

    private Player _player;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            _player.CollectPistol();
        }
        Destroy(gameObject);
    }
}
