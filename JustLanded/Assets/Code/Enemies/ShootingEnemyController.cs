using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemyController : MonoBehaviour
{
    [SerializeField] float respawnDelay = 1f;

    private Vector2 respawnPosition;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        respawnPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Die());
        }
    }


    private IEnumerator Die()
    {
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(respawnDelay);
        spriteRenderer.enabled = true;
        transform.position = (Vector2)respawnPosition;
    }

}
