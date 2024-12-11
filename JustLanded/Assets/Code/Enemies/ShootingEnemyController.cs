using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemyController : MonoBehaviour
{
    [SerializeField] float respawnDelay = 1f;
    [SerializeField] GameObject spit;
    [SerializeField] Transform spitSpawnPoint;
    [SerializeField] GameObject player;
    [SerializeField] float secondBetweenShots;

    private AudioSource audioSource;
    private float aimAngle;

    private Vector2 respawnPosition;
    private float lastShotTime;
    private SpriteRenderer spriteRenderer;
    private bool isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        respawnPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = (Vector2)(player.transform.position - transform.position).normalized;
        aimAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    void FixedUpdate()
    {
        if (CanShootAnotherBullet())
        {
            lastShotTime = Time.time;
            Instantiate(spit, spitSpawnPoint.position, Quaternion.Euler(0, 0, aimAngle));
        }

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
        audioSource.Play();
        spriteRenderer.enabled = false;
        isAlive = false;
        yield return new WaitForSeconds(respawnDelay);
        spriteRenderer.enabled = true;
        isAlive = true;
        transform.position = (Vector2)respawnPosition;
    }

    private bool CanShootAnotherBullet()
    {
        return isAlive && (Time.time - lastShotTime) > secondBetweenShots;
    }
}
