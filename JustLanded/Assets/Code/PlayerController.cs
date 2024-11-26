using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public InputAction LeftAction;
    private Rigidbody2D rigidbody;

    //Vector2 move;
    //public float speed = 0.1f;


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
        Vector2 position = (Vector2)rigidbody.position + move;
        // * Time.deltaTime;
        rigidbody.MovePosition(position);

        if (Gamepad.current.aButton.isPressed)
        {
            rigidbody.AddForce(new Vector2(0, 1), ForceMode2D.Impulse);
        }

    }

    // FixedUpdate has the same call rate as the physics system
    // void FixedUpdate()
    // {
    //     // Vector2 position = (Vector2)rigidbody.position + move * Time.deltaTime;
    //     // rigidbody.MovePosition(position);
    // }
}
