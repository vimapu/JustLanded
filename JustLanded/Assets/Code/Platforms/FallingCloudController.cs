using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingCloudController : MonoBehaviour
{

    [SerializeField] float fallDelay = 1f;
    [SerializeField] float respawnDelay = 1f;
    private bool _isFalling = false;

    private Rigidbody2D _rigidbody;
    private Vector2 _respawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        _respawnPosition = transform.position;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isFalling) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }

    private IEnumerator Fall()
    {
        _isFalling = true;
        yield return new WaitForSeconds(fallDelay);
        _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        
        yield return new WaitForSeconds(respawnDelay);
        _isFalling = false;
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.bodyType = RigidbodyType2D.Kinematic;
        transform.position = (Vector2) _respawnPosition;

    }


}
