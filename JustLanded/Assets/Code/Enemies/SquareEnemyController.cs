using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SquareEnemyController : MonoBehaviour, IKillable, Subject<DeadEnemyEvent>, IListener<EndOfLevelEvent>
{
    [SerializeField] Transform[] positions;
    [SerializeField] float speed = 10f;
    [SerializeField] float damage = 50f;
    [SerializeField] float points = 100f;

    private AudioSource audioSource;
    private int positionIndex = 0;
    private Transform nextPosition;
    private Vector2 direction;
    private Rigidbody2D rigidbody;
    private bool isAlive = true;
    private List<IListener<DeadEnemyEvent>> _listeners;


    void Awake()
    {
        _listeners = new List<IListener<DeadEnemyEvent>>();
    }
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        nextPosition = positions[positionIndex];
        audioSource = GetComponent<AudioSource>();
        CalculateDirection();
        List<Subject<EndOfLevelEvent>> endOfLevelSubjects = FindObjectsOfType<MonoBehaviour>(true).OfType<Subject<EndOfLevelEvent>>().ToList();
        foreach (Subject<EndOfLevelEvent> endOfLevelSubject in endOfLevelSubjects)
        {
            endOfLevelSubject.Add(this);
        }
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
            var player = other.collider.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
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
        audioSource.Play();
        Notify(new DeadEnemyEvent(points));
        Jump();
        Destroy(gameObject.transform.parent.gameObject, 0.5f);
    }

    public void Kill()
    {
        Die();
    }

    public void Add(IListener<DeadEnemyEvent> listener)
    {
        Debug.Log("Adding listener to enemy subject");
        _listeners.Add(listener);
    }

    public void Detach(IListener<DeadEnemyEvent> listener)
    {
        _listeners.Remove(listener);
    }

    public void Notify(DeadEnemyEvent notification)
    {
        foreach (IListener<DeadEnemyEvent> listener in _listeners)
        {
            listener.Notify(notification);
        }
    }

    public void Notify(EndOfLevelEvent notification)
    {
        gameObject.SetActive(false);
    }
}
