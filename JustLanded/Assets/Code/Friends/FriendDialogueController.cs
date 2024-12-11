using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendDialogueController : MonoBehaviour
{

    [SerializeField] GameObject dialogue;

    private bool hasActivatedDialogue = false;


    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") && !hasActivatedDialogue)
        {
            dialogue.SetActive(true);
        }
    }
}
