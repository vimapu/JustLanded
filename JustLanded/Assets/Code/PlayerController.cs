
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public InputAction LeftAction;
    Rigidbody2D rigidbody;

    Vector2 move;
    public float speed = 10f;


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
        //Vector2 position = (Vector2)transform.position + move;
        // * Time.deltaTime;
        //transform.position = position;

        if (Gamepad.current.aButton.isPressed)
        {
            Debug.Log("Pressing A button");
            rigidbody.AddForce(new Vector2(0, 100), ForceMode2D.Force);
        }

    }

    // FixedUpdate has the same call rate as the physics system
    void FixedUpdate()
    {
        Vector2 position = (Vector2)rigidbody.position + move * speed * Time.deltaTime;
//        transform.position = position;
        rigidbody.MovePosition(position);
    }
}
