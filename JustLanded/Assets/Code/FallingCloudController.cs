using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingCloudController : MonoBehaviour
{

    [SerializeField] float fallDelay = 1f;
    [SerializeField] float respawnDelay = 1f;
    private bool isFalling = false;

    private Rigidbody2D rigidbody;
    private Vector2 respawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        respawnPosition = transform.position;
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isFalling) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }

    private IEnumerator Fall()
    {
        isFalling = true;
        yield return new WaitForSeconds(fallDelay);
        rigidbody.bodyType = RigidbodyType2D.Dynamic;
        
        yield return new WaitForSeconds(respawnDelay);
        isFalling = false;
        rigidbody.velocity = Vector2.zero;
        rigidbody.bodyType = RigidbodyType2D.Kinematic;
        transform.position = (Vector2) respawnPosition;

    }


}
