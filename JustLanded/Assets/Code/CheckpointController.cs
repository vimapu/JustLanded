using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<DeathAndRespawnController>();
        if (player != null)
        {
            player.SetRespawnPosition(transform.position);
        }
    }
}
