using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BashController : MonoBehaviour
{

    [SerializeField] float bashingSpeed = 40f;
    [SerializeField] float bashingTime = 0.5f;
    private MovementController movementController;
    private Rigidbody2D rigidbody;

    private bool isBashing = false;
    private bool hasBashBeenReleased = true;
    private float bashStartTime;

    // Start is called before the first frame update
    void Start()
    {
        movementController = GetComponent<MovementController>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isBashPressed = Gamepad.current.bButton.isPressed;
        if (!IsBashing() && hasBashBeenReleased && isBashPressed)
        {
            hasBashBeenReleased = false;
            StartBash();
        }
        if (!hasBashBeenReleased && !isBashPressed)
        {
            hasBashBeenReleased = true;
        }
    }

    void FixedUpdate()
    {
        if (IsBashing())
        {
            var speed = movementController.IsFacingRight()? bashingSpeed: -bashingSpeed;
            rigidbody.velocity = new Vector2(speed, rigidbody.velocity.y);
        }
    }

    void StartBash()
    {
        isBashing = true;
        bashStartTime = Time.time;
    }

    void FinishBash()
    {
        isBashing = false;
    }

    public bool IsBashing()
    {
        return isBashing && (Time.time - bashStartTime) < bashingTime;
    }


}
