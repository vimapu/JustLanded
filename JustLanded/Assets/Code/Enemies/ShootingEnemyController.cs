using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemyController : MonoBehaviour, IKillable, Subject<DeadEnemyEvent>
{
    [SerializeField] float respawnDelay = 1f;
    [SerializeField] GameObject spit;
    [SerializeField] Transform spitSpawnPoint;
    [SerializeField] GameObject player;
    [SerializeField] float secondBetweenShots;
    [SerializeField] bool doesRespawn = false;
    [SerializeField] float points = 0;

    private AudioSource audioSource;
    private float aimAngle;

    private Vector2 respawnPosition;
    private float lastShotTime;
    private SpriteRenderer spriteRenderer;
    private bool isAlive = true;
    private List<IListener<DeadEnemyEvent>> Listeners;

    // Start is called before the first frame update
    void Start()
    {
        respawnPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        //player = GameObject.Find("Player");
        Listeners = new List<IListener<DeadEnemyEvent>>();
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

    //  private void OnCollisionEnter2D(Collision2D other)
    // {
    //     if (isAlive)
    //     {
    //         var player = other.collider.GetComponent<DeathAndRespawnController>();
    //         if (player != null)
    //         {
    //             player.Kill(this);
    //         }
    //     }

    // }

    void OnTriggerEnter2D(Collider2D collider)
    {
        var player = collider.GetComponent<Player>();
        if (player != null)
        {
            player.Kill(this);
        }
    }

    public void Kill()
    {
        StartCoroutine(Die());
    }


    private IEnumerator Die()
    {
        audioSource.Play();
        spriteRenderer.enabled = false;
        isAlive = false;
        Notify(new DeadEnemyEvent(points));
        if (doesRespawn)
        {
            yield return new WaitForSeconds(respawnDelay);
            spriteRenderer.enabled = true;
            isAlive = true;
            transform.position = (Vector2)respawnPosition;
        }
        else
        {
            Destroy(gameObject, 0.5f);
        }

    }

    private bool CanShootAnotherBullet()
    {
        return isAlive && (Time.time - lastShotTime) > secondBetweenShots;
    }

    public void Add(IListener<DeadEnemyEvent> listener)
    {
        Debug.Log("Adding listener to enemy subject");
        Listeners.Add(listener);
    }

    public void Detach(IListener<DeadEnemyEvent> listener)
    {
        Listeners.Remove(listener);
    }

    public void Notify(DeadEnemyEvent notification)
    {
        foreach (IListener<DeadEnemyEvent> listener in Listeners)
        {
            listener.Notify(notification);
        }
    }
}
