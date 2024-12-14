using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendDialogueController : MonoBehaviour
{

    [SerializeField] GameObject dialogue;
    [SerializeField] string[] dialogueLines; 

    private bool hasActivatedDialogue = false;


    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") && !hasActivatedDialogue)
        {
            hasActivatedDialogue = true;
            dialogue.SetActive(true);
            var dialogueController = dialogue.GetComponent<DialogueController>();
            dialogueController.SetLines(dialogueLines);
            dialogueController.StartDialogue();
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            dialogue.SetActive(false);
        }
    }
}
