using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LadderClimbing : MonoBehaviour
{

    private bool canClimb;
    private float vertical;
    private float speed = 8f;
    [SerializeField] InputAction climbingAction;

    [SerializeField] Rigidbody2D rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        climbingAction.Enable();
        canClimb = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canClimb)
        {
            vertical = climbingAction.ReadValue<float>();
        }
    }

    void FixedUpdate()
    {
        if (canClimb)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, vertical * speed);
        }
    }

    public void EnableClimbing()
    {
        canClimb = true;
    }

    public void DisableClimbing()
    {
        canClimb = false;
        rigidbody.gravityScale = 1f;
    }



}
