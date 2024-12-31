using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringController : MonoBehaviour
{
    [SerializeField] float jumpSpeed;

    private AudioSource audioSource;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            audioSource.Play();
            var jumpingController = collider.gameObject.GetComponent<Player>();
            jumpingController.Jump(jumpSpeed);
        }
    }
}
