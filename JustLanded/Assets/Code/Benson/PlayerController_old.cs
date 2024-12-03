
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

public class PlayerController : MonoBehaviour
{

    public InputAction LeftAction;
    Rigidbody2D rigidbody;


    Vector2 move;
    [Header("Movement system")]
    [SerializeField] float acceleration = 100f;
    [SerializeField] float maxVelocity = 10f;


    // Start is called before the first frame update
    void Start()
    {
        LeftAction.Enable();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        move = LeftAction.ReadValue<Vector2>();
    }

    // FixedUpdate has the same call rate as the physics system
    void FixedUpdate()
    {
        float targetVel;
        if (Mathf.Abs(move.x) > 0.1f)
        {
            targetVel = rigidbody.velocity.x + (move.x * acceleration * Time.deltaTime);
        }
        else
        {
            targetVel = rigidbody.velocity.x - (Mathf.Sign(rigidbody.velocity.x) * acceleration/2 * Time.deltaTime);
        }
        targetVel = (Mathf.Abs(targetVel) > maxVelocity) ? maxVelocity : targetVel;
        rigidbody.velocity = new Vector2(targetVel, rigidbody.velocity.y);
    }
}
