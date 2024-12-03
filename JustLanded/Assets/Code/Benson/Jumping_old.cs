
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpingOld : MonoBehaviour
{
     Rigidbody2D rigidbody;

    [Header("Jump System")]
    [SerializeField] float jumpTime;
    [SerializeField] int jumPower;
    [SerializeField] float fallMultiplier;
    [SerializeField] float jumpMultiplier;
    
    public Transform groundCheck;
    public LayerMask groundLayer;
    bool isGrounded;
    Vector2 vecGravity;

    bool isJumping;
    float jumpCounter;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        vecGravity = new Vector2(0, Physics2D.gravity.y);
    }

    // Update is called once per frame
    void Update()
    {
         isGrounded = Physics2D.OverlapCapsule(rigidbody.position, new Vector2(2.4f, 2.4f), CapsuleDirection2D.Horizontal, 0, groundLayer);

        if (Gamepad.current.aButton.isPressed && isGrounded)
        {
            //Debug.Log("Pressing A button");
            //rigidbody.AddForce(new Vector2(0, 1), ForceMode2D.Impulse);
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumPower);
            isJumping = true;
            jumpCounter = 0;
        }

        if (rigidbody.velocity.y > 0 && isJumping)
        {
            jumpCounter += Time.deltaTime;
            if (jumpCounter > jumpTime) isJumping = false;
            float t = jumpCounter / jumpTime;
            float currentJumpM = jumpMultiplier;

            if (t > 0.5)
            {
                currentJumpM = jumpMultiplier * (1 - t);
            }

            rigidbody.velocity -= vecGravity * currentJumpM * Time.deltaTime;
        }
        if (!Gamepad.current.aButton.isPressed)
        {
            isJumping = false;
            jumpCounter = 0;

            if (rigidbody.velocity.y > 0)
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, rigidbody.velocity.y * 0.6f);
            }
        }

        // makes the player fall faster after 
        if (rigidbody.velocity.y < 0)
        {
            rigidbody.velocity += vecGravity * fallMultiplier * Time.deltaTime;
        }
    }
}
