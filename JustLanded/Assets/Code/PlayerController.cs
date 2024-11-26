using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public InputAction LeftAction;


    // Start is called before the first frame update
    void Start()
    {
        LeftAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("This is a log message");
        {
            Vector2 move = LeftAction.ReadValue<Vector2>();
            Debug.Log(move);
            Vector2 position = (Vector2)transform.position + move * 0.1f;
            transform.position = position;
        }
    }
}
