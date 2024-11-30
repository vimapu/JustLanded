using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        var player = other.collider.GetComponent<DeathAndRespawnController>();
        if (player != null)
        {
            player.Die();
        }
    }
}
