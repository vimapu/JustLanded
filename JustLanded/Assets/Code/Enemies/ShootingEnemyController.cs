using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShootingEnemyController : MonoBehaviour, IKillable, ISubject<DeadEnemyEvent>, IListener<EndOfLevelEvent>, IListener<PlayerDeathEvent>
{
    [SerializeField] float RespawnDelay = 1f;
    [SerializeField] GameObject Spit;
    [SerializeField] Transform SpitSpawnPoint;
    [SerializeField] GameObject Player;
    [SerializeField] float SecondBetweenShots = 2f;
    [SerializeField] bool DoesRespawn = false;
    [SerializeField] float Points = 200f;
    [SerializeField] float Damage = 75f;

    private AudioSource _audioSource;
    private float _aimAngle;
    private bool _hasDied = false;
    private Vector2 _respawnPosition;
    private float _lastShotTime;
    private SpriteRenderer _spriteRenderer;
    private bool _isAlive = true;
    private List<IListener<DeadEnemyEvent>> _listeners;

    void Awake()
    {
        _listeners = new List<IListener<DeadEnemyEvent>>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _respawnPosition = transform.position;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();

        List<ISubject<EndOfLevelEvent>> endOfLevelSubjects = FindObjectsOfType<MonoBehaviour>(true).OfType<ISubject<EndOfLevelEvent>>().ToList();
        foreach (ISubject<EndOfLevelEvent> endOfLevelSubject in endOfLevelSubjects)
        {
            endOfLevelSubject.Add(this);
        }
        List<ISubject<PlayerDeathEvent>> playerDeathSubjects = FindObjectsOfType<MonoBehaviour>(true).OfType<ISubject<PlayerDeathEvent>>().ToList();
        foreach (ISubject<PlayerDeathEvent> playerDeathSubject in playerDeathSubjects)
        {
            playerDeathSubject.Add(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = (Vector2)(Player.transform.position - transform.position).normalized;
        _aimAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    void FixedUpdate()
    {
        if (CanShootAnotherBullet())
        {
            _lastShotTime = Time.time;
            Instantiate(Spit, SpitSpawnPoint.position, Quaternion.Euler(0, 0, _aimAngle));
        }

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        var player = collider.GetComponent<Player>();
        if (player != null && _isAlive)
        {
            player.TakeDamage(Damage, this);
        }
    }

    public void Kill()
    {
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        _audioSource.Play();
        _spriteRenderer.enabled = false;
        _isAlive = false;
        if (!_hasDied)
        {
            Notify(new DeadEnemyEvent(Points));
            _hasDied = true;
        }
        if (DoesRespawn)
        {
            yield return new WaitForSeconds(RespawnDelay);
            _spriteRenderer.enabled = true;
            _isAlive = true;
            transform.position = (Vector2)_respawnPosition;
        }
        else
        {
            Deactivate();
        }

    }

    IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

    void Reactivate()
    {
        gameObject.SetActive(true);
        _spriteRenderer.enabled = true;
        _isAlive = true;
    }

    private bool CanShootAnotherBullet()
    {
        return _isAlive && (Time.time - _lastShotTime) > SecondBetweenShots;
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
}
