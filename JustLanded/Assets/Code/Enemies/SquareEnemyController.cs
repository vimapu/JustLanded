using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SquareEnemyController : MonoBehaviour, IKillable, Subject<DeadEnemyEvent>, IListener<EndOfLevelEvent>, IListener<PlayerDeathEvent>
{
    [SerializeField] Transform[] positions;
    [SerializeField] float Speed = 10f;
    [SerializeField] float Damage = 50f;
    [SerializeField] float Points = 100f;

    private AudioSource _audioSource;
    private int _positionIndex = 0;
    private Transform _nextPosition;
    private Vector2 _direction;
    private Rigidbody2D _rigidbody;
    private bool _isAlive = true;
    private bool _hasDied = false;
    private Vector2 _initialPosition;
    private List<IListener<DeadEnemyEvent>> _listeners;


    void Awake()
    {
        _listeners = new List<IListener<DeadEnemyEvent>>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _nextPosition = positions[_positionIndex];
        _audioSource = GetComponent<AudioSource>();
        _initialPosition = transform.position;
        CalculateDirection();
        List<Subject<EndOfLevelEvent>> endOfLevelSubjects = FindObjectsOfType<MonoBehaviour>(true).OfType<Subject<EndOfLevelEvent>>().ToList();
        foreach (Subject<EndOfLevelEvent> endOfLevelSubject in endOfLevelSubjects)
        {
            endOfLevelSubject.Add(this);
        }
        List<Subject<PlayerDeathEvent>> playerDeathSubjects = FindObjectsOfType<MonoBehaviour>(true).OfType<Subject<PlayerDeathEvent>>().ToList();
        foreach (Subject<PlayerDeathEvent> playerDeathSubject in playerDeathSubjects)
        {
            playerDeathSubject.Add(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(_nextPosition.position, transform.position) < 0.2f)
        {
            if (_positionIndex + 1 >= positions.Length)
            {
                _positionIndex = 0;
            }
            else
            {
                _positionIndex++;
            }
            _nextPosition = positions[_positionIndex];
        }
        CalculateDirection();
    }

    void FixedUpdate()
    {
        // starts moving towards the next position
        if (_isAlive)
        {
            _rigidbody.velocity = _direction * Speed;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_isAlive)
        {
            var player = other.collider.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(Damage);
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
        _direction = (_nextPosition.position - transform.position).normalized;
    }

    private void Jump()
    {
        _rigidbody.velocity = new Vector2(0f, 5f);
    }

    public void Die()
    {
        BoxCollider2D[] colliders = GetComponents<BoxCollider2D>();
        for (int i = 0; i < colliders.Length; i++)
        {
            BoxCollider2D collider = (BoxCollider2D)colliders.GetValue(i);
            collider.enabled = false;
        }
        _isAlive = false;
        _audioSource.Play();
        if (!_hasDied)
        {
            Notify(new DeadEnemyEvent(Points));
            _hasDied = true;
        }
        Jump();
        StartCoroutine(Deactivate());
    }

    IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.transform.parent.gameObject.SetActive(false);
    }

    public void Kill()
    {
        Die();
    }

    public void Add(IListener<DeadEnemyEvent> listener)
    {
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

    public void Notify(PlayerDeathEvent notification)
    {
        Reactivate();
    }

    void Reactivate()
    {
        transform.position = _initialPosition;
        gameObject.transform.parent.gameObject.SetActive(true);
        BoxCollider2D[] colliders = GetComponents<BoxCollider2D>();
        for (int i = 0; i < colliders.Length; i++)
        {
            BoxCollider2D collider = (BoxCollider2D)colliders.GetValue(i);
            collider.enabled = true;
        }
        _isAlive = true;
    }
}
