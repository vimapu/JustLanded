
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{

    [Header("Movement params")]
    [SerializeField] float speed;
    [SerializeField] InputAction inputAction;

    Rigidbody2D rigidbody;
    
    Vector2 movement;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        inputAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {   
        movement = inputAction.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        rigidbody.velocity = new Vector2(movement.x * speed, rigidbody.velocity.y);
    }

}
