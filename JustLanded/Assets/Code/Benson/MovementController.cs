
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{

    [Header("Movement params")]
    [SerializeField] float speed;
    [SerializeField] InputAction inputAction;

    Rigidbody2D rigidbody;

    Vector2 movement;

    GunController gunController;

    bool isFacingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        inputAction.Enable();
        gunController = GetComponent<GunController>();
    }

    // Update is called once per frame
    void Update()
    {
        movement = inputAction.ReadValue<Vector2>();
        if ((isFacingRight && movement.x < 0f) || (!isFacingRight && movement.x > 0f))
        {
            Flip();
        }
    }

    void FixedUpdate()
    {
        rigidbody.velocity = new Vector2(movement.x * speed, rigidbody.velocity.y);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        var localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
        gunController.Flip();
    }

}
