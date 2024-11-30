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
            var pos = (Vector2)transform.position;
            Debug.Log("setting respawn position" + pos);
            player.SetRespawnPosition(pos);
        }
    }
}
