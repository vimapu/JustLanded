using System.Collections;

using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;


public class DialogueController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textComponent;
    [SerializeField] string[] lines;
    [SerializeField] float textSpeed;
    private int index;
    private bool hasStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current.aButton.isPressed)
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            // else
            // {
            //     Debug.Log("Stopping coroutines");
            //     StopAllCoroutines();
            //     textComponent.text = lines[index];
            // }

        }
    }

    void StartDialogue()
    {
        index = 0;
        hasStarted = true;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        Debug.Log("printing line: " + lines[index]);
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            Debug.Log("New line");
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
            index = 0;
        }
    }
}
