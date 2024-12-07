using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{

    [SerializeField] Transform[] positions;
    [SerializeField] float gravityOnPlatform = 50;
    [SerializeField] float speed = 10f;



    private Transform nextPosition;
    private int positionIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        nextPosition = positions[positionIndex];
        GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(nextPosition.position, transform.position) < 0.1f)
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
    }

    void FixedUpdate()
    {
        // starts moving towards the next position
        var step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, nextPosition.position, step);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.parent = transform;
            collision.rigidbody.gravityScale = collision.rigidbody.gravityScale * gravityOnPlatform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.parent = null;
            collision.rigidbody.gravityScale = collision.rigidbody.gravityScale / gravityOnPlatform;
        }
    }
}
