using System.Collections;

using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;


public class DialogueController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textComponent;
    [SerializeField] float textSpeed;
    private int _index;
    private bool _hasStarted = false;
    private string[] lines;

    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current.aButton.isPressed)
        {
            if (textComponent.text == lines[_index])
            {
                NextLine();
            }
        }
    }

    public void StartDialogue()
    {
        _index = 0;
        _hasStarted = true;
        textComponent.text = string.Empty;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[_index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (_index < lines.Length - 1)
        {
            _index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
            _index = 0;
        }
    }

    public void SetLines(string[] lines)
    {
        this.lines = lines;
    }
}
