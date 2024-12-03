
using UnityEngine;
using UnityEngine.InputSystem;

public class Jumping : MonoBehaviour
{
    Rigidbody2D rigidbody;

    [Header("Jump System")]
    [SerializeField] int jumpPower;
    [SerializeField] float jumpPowerPercentWhenReleased;

    public LayerMask groundLayer;

    private bool isAPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        isAPressed = Gamepad.current.aButton.isPressed;
    }

    void FixedUpdate()
    {
        if (isAPressed && IsGrounded())
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpPower);
        }
        if (!isAPressed && rigidbody.velocity.y > 0f)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, rigidbody.velocity.y * jumpPowerPercentWhenReleased);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCapsule(rigidbody.position, new Vector2(2.4f, 2.4f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }
}
