
using UnityEngine;
using UnityEngine.InputSystem;

public class Jumping : MonoBehaviour
{
    Rigidbody2D rigidbody;

    [Header("Jump System")]
    [SerializeField] int jumpPower;
    [SerializeField] float jumpPowerPercentWhenReleased;

    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] Transform wallCheckRight;
    [SerializeField] Transform wallCheckLeft;

    private bool isAPressed = false;

    private bool canDoubleJump = false;
    private bool hasLearnedDoubleJump = false;

    private bool isJumping = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        isAPressed = Gamepad.current.aButton.isPressed;
        if (!isAPressed && isJumping && hasLearnedDoubleJump)
        {
            canDoubleJump = true;
        }
    }

    void FixedUpdate()
    {
        if (isAPressed)
        {
            if (IsGrounded() || IsWalled())
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpPower);
                isJumping = true;
            }
            else
            {
                if (canDoubleJump)
                {
                    rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpPower);
                    canDoubleJump = false;
                }
            }
        }
        if (!isAPressed && rigidbody.velocity.y > 0f)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, rigidbody.velocity.y * jumpPowerPercentWhenReleased);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (IsGrounded() || IsWalled())
        {
            Debug.Log("Not jumping");
            isJumping = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsGrounded() || IsWalled())
        {
            Debug.Log("Not jumping");
            isJumping = false;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCapsule(rigidbody.position, new Vector2(2.4f, 2.4f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheckRight.position, 0.5f, wallLayer)
        || Physics2D.OverlapCircle(wallCheckLeft.position, 0.5f, wallLayer);
    }

    public void LearnDoubleJump()
    {
        hasLearnedDoubleJump = true;
    }
}
