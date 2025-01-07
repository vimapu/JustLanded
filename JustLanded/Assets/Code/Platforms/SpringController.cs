using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringController : MonoBehaviour
{
    [SerializeField] float JumpSpeed = 100f;

    private AudioSource _audioSource;

    void Start() {
        _audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            _audioSource.Play();
            var player = collider.gameObject.GetComponent<Player>();
            player.Jump(JumpSpeed);
        }
    }
}
