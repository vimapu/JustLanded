
using UnityEngine;


public class MovingCloudController : MonoBehaviour
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
    private bool facingLeft = true;


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
            FlipIfNecessary();
        }
        CalculateDirection();

    }
    void FixedUpdate()
    {
        // starts moving towards the next position
        rigidbody.velocity = direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Entering platform");
            //collider.transform.parent = transform;
            //movementController.SetPlatformRB(rigidbody);
            //playerRigidbody.gravityScale = playerRigidbody.gravityScale * gravityOnPlatform;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Exiting platform");
           // collider.transform.parent = null;
            //movementController.LeavePlatform();
            //playerRigidbody.gravityScale = playerRigidbody.gravityScale / gravityOnPlatform;
        }
    }

    private void CalculateDirection()
    {
        direction = (nextPosition.position - transform.position).normalized;
    }

    private void FlipIfNecessary()
    {
        CalculateDirection();
        if ((direction.x > 0 && facingLeft) || (direction.x < 0 && !facingLeft))
        {
            facingLeft = !facingLeft;
            var localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
