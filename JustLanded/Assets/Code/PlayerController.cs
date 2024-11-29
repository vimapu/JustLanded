
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public InputAction LeftAction;
    Rigidbody2D rigidbody;
    [SerializeField] int jumPower;

    public Transform groundCheck;
    public LayerMask groundLayer;
    bool isGrounded;

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
        isGrounded = Physics2D.OverlapCapsule(rigidbody.position, new Vector2(2.4f, 2.4f), CapsuleDirection2D.Horizontal, 0, groundLayer );

        if (Gamepad.current.aButton.isPressed && isGrounded)
        {
            Debug.Log("Pressing A button");
            //rigidbody.AddForce(new Vector2(0, 1), ForceMode2D.Impulse);
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumPower);

        }

    }

    // FixedUpdate has the same call rate as the physics system
    void FixedUpdate()
    {
        //Vector2 position = (Vector2)rigidbody.position + move * speed * Time.deltaTime;
//        transform.position = position;
        //rigidbody.MovePosition(position);
    }
}
