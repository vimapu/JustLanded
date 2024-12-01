using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderController : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var player = collider.GetComponent<LadderClimbing>();
        if (player != null)
        {
            player.EnableClimbing();
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        var player = collider.GetComponent<LadderClimbing>();
        if (player != null)
        {
            player.DisableClimbing();
        }
    }
}
