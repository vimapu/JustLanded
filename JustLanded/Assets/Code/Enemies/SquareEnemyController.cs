using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareEnemyController : MonoBehaviour
{
    [SerializeField] Transform[] positions;
    [SerializeField] float speed = 10f;

    private int positionIndex = 0;
    private Transform nextPosition;
    private Vector2 direction;
    private Rigidbody2D rigidbody;
    private bool isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        nextPosition = positions[positionIndex];
        CalculateDirection();
    }

    // Update is called once per frame
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
        if (isAlive)
        {
            rigidbody.velocity = direction * speed;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isAlive)
        {
            var player = other.collider.GetComponent<DeathAndRespawnController>();
            if (player != null)
            {
                player.Die();
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Die();
        }
    }

    private void CalculateDirection()
    {
        direction = (nextPosition.position - transform.position).normalized;
    }

    private void Jump()
    {
        rigidbody.velocity = new Vector2(0f, 5f);
    }

    public void Die()
    {
        BoxCollider2D[] colliders = GetComponents<BoxCollider2D>();
        for (int i = 0; i < colliders.Length; i++)
        {
            BoxCollider2D collider = (BoxCollider2D)colliders.GetValue(i);
            collider.enabled = false;
        }
        isAlive = false;
        Jump();
        Destroy(gameObject.transform.parent.gameObject, 0.5f);
    }
}
