using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{

    [SerializeField] Transform[] positions;
    [SerializeField] float gravityOnPlatform = 5;
    [SerializeField] float speed = 10f;
    [SerializeField] Rigidbody2D playerRigidbody;


    MovementController movementController;
    private Transform nextPosition;
    private int positionIndex = 0;
    private Rigidbody2D rigidbody;
    private Vector2 direction;


    // Start is called before the first frame update
    void Start()
    {
        movementController = GameObject.FindGameObjectWithTag("Player").GetComponent<MovementController>();
        nextPosition = positions[positionIndex];
        rigidbody = GetComponent<Rigidbody2D>();
        CalculateDirection();
    }

    void Update()
    {
        if (Vector2.Distance(nextPosition.position, transform.position) < 0.2f)
        {
            if (positionIndex + 1 >= positions.Length)
            {
                positionIndex = 0;
            }
            else
            {
                positionIndex++;
            }
            nextPosition = positions[positionIndex];
        }
        CalculateDirection();

    }
    void FixedUpdate()
    {
        // starts moving towards the next position
        rigidbody.velocity = direction * speed;
    }

    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         collision.transform.parent = transform;
    //         movementController.SetPlatformRB(rigidbody);
    //         //playerRigidbody.gravityScale = playerRigidbody.gravityScale * gravityOnPlatform;
    //     }
    // }

    // private void OnCollisionExit2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         collision.transform.parent = null;
    //         movementController.LeavePlatform();
    //         //playerRigidbody.gravityScale = playerRigidbody.gravityScale / gravityOnPlatform;
    //     }
    // }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            collider.transform.parent = transform;
            movementController.SetPlatformRB(rigidbody);
            //playerRigidbody.gravityScale = playerRigidbody.gravityScale * gravityOnPlatform;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            collider.transform.parent = null;
            movementController.LeavePlatform();
            //playerRigidbody.gravityScale = playerRigidbody.gravityScale / gravityOnPlatform;
        }
    }

    private void CalculateDirection()
    {
        direction = (nextPosition.position - transform.position).normalized;
    }
}
