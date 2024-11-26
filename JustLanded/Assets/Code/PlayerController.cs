using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public InputAction LeftAction;
    private Rigidbody2D rigidbody;


    // Start is called before the first frame update
    void Start()
    {
        LeftAction.Enable();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 move = LeftAction.ReadValue<Vector2>();
        Debug.Log(move);
        Vector2 position = (Vector2)transform.position + move * 0.1f;
        transform.position = position;

        if (Gamepad.current.aButton.isPressed)
        {
            Debug.Log("The X button is pressed.");
            rigidbody.AddForce(new Vector2(0, 1), ForceMode2D.Impulse);
        }

    }
}
