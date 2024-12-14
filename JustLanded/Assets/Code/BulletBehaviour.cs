using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{

    [SerializeField] float speed = 20f;
    [SerializeField] float expirationTime = 3f;
    [SerializeField] LayerMask whatDestroysBullet;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SetStraightVelocity();
        SetDestroyTime();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("SquareEnemy"))
        {
            collider.gameObject.GetComponent<SquareEnemyController>().Kill();
        }
        else if (collider.gameObject.CompareTag("TriangularEnemy"))
        {
            collider.gameObject.GetComponent<ShootingEnemyController>().Kill();
        }
        if ((whatDestroysBullet.value & (1 << collider.gameObject.layer)) > 0)
        {
            Debug.Log("Destroying bullet");
            Destroy(gameObject);
        }
    }

    private void SetStraightVelocity()
    {
        rb.velocity = transform.right * speed;
    }

    private void SetDestroyTime()
    {
        Destroy(gameObject, expirationTime);
    }
}
