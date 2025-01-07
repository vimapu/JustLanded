using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour, IListener<EndOfLevelEvent>, IListener<HealthItemCollectedEvent>, Subject<PlayerDeathEvent>
{

    [Header("Bashing parameters")]
    [SerializeField] float BashingSpeed = 40f;
    [SerializeField] float BashingTime = 0.5f;

    [Header("Movement parameters")]
    [SerializeField] float MovementSpeed;
    [SerializeField] InputAction InputAction;
    [SerializeField] private CircleCollider2D playerCollider;

    [Header("Gun parameters")]
    [SerializeField] GameObject Pistol;
    [SerializeField] float SecondBetweenShots;
    [SerializeField] GameObject Bullet;
    [SerializeField] Transform BulletSpawnPoint;

    [Header("Respawning parameters")]
    [SerializeField] float RespawnJumpSpeed = 10f;

    [Header("Jump parameters")]
    [SerializeField] float JumpPower = 10f;
    [SerializeField] float JumpPowerPercentWhenReleased;
    [SerializeField] LayerMask GroundLayer;
    [SerializeField] LayerMask WallLayer;
    [SerializeField] Transform WallCheckRight;
    [SerializeField] Transform WallCheckLeft;

    [Header("Platform parameters")]
    [SerializeField] LayerMask PlatformLayer;

    [Header("Ladder parameters")]
    [SerializeField] float LadderSpeed;

    [Header("Health Bar parameters")]
    [SerializeField] Image HealthBar;
    [SerializeField] float MaxHealthAmount = 100f;

    private List<IListener<PlayerDeathEvent>> _listeners;
    private GameObject _pistol;

    private Rigidbody2D _rigidbody;
    public Rigidbody2D Rigidbody { get { return _rigidbody; } }

    private Collider2D _collider;

    private StateContext stateContext;
    // states' declarations
    private IState _bashingState;
    public IState BashingState { get { return _bashingState; } }
    private IState _onAirState;
    public IState OnAirState { get { return _onAirState; } }
    private IState _OnLadderState;
    public IState OnStairsState { get { return _OnLadderState; } }
    private IState _onSurfaceState;
    public IState OnSurfaceState { get { return _onSurfaceState; } }
    private IState _onPlatformState;
    public IState OnPlatformState { get { return _onPlatformState; } }

    // bashing attributes
    private bool _isBashing = false;
    private bool _hasBashBeenReleased = true;
    private float _bashStartTime;
    bool _hasPistol = false;

    bool _isFacingRight = true;

    // gun attributes
    private GameObject _bulletInstance;
    private Vector2 _direction; //TODO: remove for unified direction vector
    private bool _isFlipped;
    private bool _isInputPressed = false;
    private float _lastShotTime;
    private bool _isGunActive = false;

    // platform attributes
    private GameObject _currentOneWayPlatform;

    // ladder attributes
    private bool _canClimb;
    private float _vertical;
    private float _gravityScale;
    private bool _isInLadder = false;
    public bool IsInLadder { get { return _isInLadder; } set { _isInLadder = value; } }

    // death and respawning attributes
    private bool _isDead = false;
    private Vector2 _respawnPosition;
    // jump attributes
    private bool canDoubleJump = false;
    private bool hasLearnedDoubleJump = false;
    private bool _isOnAir = false;
    public bool IsOnAir { get { return _isOnAir; } set { _isOnAir = value; } }
    private bool _hasDoubleJumped = false;
    public bool HasDoubleJumped { get { return _hasDoubleJumped; } set { _hasDoubleJumped = value; } }

    // health attributes
    private float _healthAmount;

    // gamepad controller attributes
    private bool _isAButtonPressed = false;
    public bool IsAButtonPressed { get { return _isAButtonPressed; } }
    private bool _isBButtonPressed = false;
    public bool IsBButtonPressed { get { return _isBButtonPressed; } }
    private bool _isYButtonPressed = false;
    public bool IsYButtonPressed { get { return _isYButtonPressed; } }
    private bool _isRightTriggerPressed;
    public bool IsRightTriggerPressed { get { return _isRightTriggerPressed; } }
    private Vector2 _leftStickDirection;
    public Vector2 LeftStickDirection { get { return _leftStickDirection; } }
    private bool _isLeftStickPressed;
    public bool IsLeftStickPressed { get { return _isLeftStickPressed; } }


    void Awake()
    {
        _listeners = new List<IListener<PlayerDeathEvent>>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _onAirState = new BaseStateDecorator(this, new OnAirState(this));
        _OnLadderState = new BaseStateDecorator(this, new OnLadderState(this));
        _onSurfaceState = new BaseStateDecorator(this, new OnSurfaceState(this));
        _bashingState = new BaseStateDecorator(this, new BashingState(this));
        _onPlatformState = new BaseStateDecorator(this, new OnPlatformState(this));
        stateContext = new StateContext(_onAirState);
        InputAction.Enable();
        _pistol = GameObject.Find("Pistol");
        _pistol.SetActive(false);
        _canClimb = false;
        _gravityScale = GetComponent<Rigidbody2D>().gravityScale;
        _collider = GetComponent<Collider2D>();
        _healthAmount = MaxHealthAmount;
        SetRespawnPosition(transform.position);
        // add listener to end of level event
        List<Subject<EndOfLevelEvent>> endOfLevelSubjects = FindObjectsOfType<MonoBehaviour>(true).OfType<Subject<EndOfLevelEvent>>().ToList();
        foreach (Subject<EndOfLevelEvent> endOfLevelSubject in endOfLevelSubjects)
        {
            endOfLevelSubject.Add(this);
        }
        List<Subject<HealthItemCollectedEvent>> healthItemSubjects = FindObjectsOfType<MonoBehaviour>(true).OfType<Subject<HealthItemCollectedEvent>>().ToList();
        foreach (Subject<HealthItemCollectedEvent> healthItemSubject in healthItemSubjects)
        {
            healthItemSubject.Add(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        RecordInput();
        stateContext.RunUpdateLogic();
    }

    void FixedUpdate()
    {
        stateContext.RunPhysicsLogic();
    }

    /* Methods to control the bashing state*/
    public void StartBash()
    {
        _isBashing = true;
        //_bashStartTime = Time.time;
    }

    public void FinishBash()
    {
        _isBashing = false;
    }

    // public bool IsBashing()
    // {
    //     return _isBashing && (Time.time - _bashStartTime) < BashingTime;
    // }

    private void RecordInput()
    {
        _leftStickDirection = InputAction.ReadValue<Vector2>();
        _isAButtonPressed = Gamepad.current.aButton.IsPressed();
        _isBButtonPressed = Gamepad.current.bButton.IsPressed();
        _isYButtonPressed = Gamepad.current.yButton.IsPressed();
        _isRightTriggerPressed = Gamepad.current.rightTrigger.IsPressed();
        _isLeftStickPressed = InputAction.IsPressed();
    }



    public void CollectPistol()
    {
        _pistol.SetActive(true);
        _hasPistol = true;
        ActivateGun();
    }


    public void Shoot()
    {
        if (_lastShotTime != null && CanShootAnotherBullet()) // TODO: remove first clause
        {
            _lastShotTime = Time.time;
            // instantiate bullet and shoot
            if (_isFacingRight)
            {
                _bulletInstance = Instantiate(Bullet, BulletSpawnPoint.position, _pistol.transform.rotation);
            }
            else
            {
                var rotation = _pistol.transform.rotation.eulerAngles;
                var eulerRotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z - 180);
                _bulletInstance = Instantiate(Bullet, BulletSpawnPoint.position, eulerRotation);
            }
        }
    }

    private bool CanShootAnotherBullet()
    {
        return (Time.time - _lastShotTime) > SecondBetweenShots;
    }
    public void UpdateGunRotation()
    {
        var aimAngle = Mathf.Atan2(LeftStickDirection.y, LeftStickDirection.x) * Mathf.Rad2Deg;
        if (_isFacingRight)
        {
            if (LeftStickDirection.x >= 0)
            {
                _pistol.transform.rotation = Quaternion.Euler(0, 0, aimAngle);
            }
        }
        else
        {
            if (LeftStickDirection.x <= 0)
            {
                _pistol.transform.rotation = Quaternion.Euler(0, 0, aimAngle - 180);
            }
        }
    }

    public void ActivateGun()
    {
        _isGunActive = true;
    }

    public void DeactivateGun()
    {
        _isGunActive = false;
    }

    public bool IsGunActive()
    {
        return _isGunActive;
    }

    public float GetJumpPower()
    {
        return JumpPower;
    }

    public float GetLadderSpeed()
    {
        return LadderSpeed;
    }

    public float GetMovementSpeed()
    {
        return MovementSpeed;
    }

    public void Flip()
    {
        _isFacingRight = !_isFacingRight;
        var localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
        _isFlipped = !_isFlipped;
        //gunController.Flip();
    }

    public void SetFacingRight()
    {
        if (!_isFacingRight)
        {
            Flip();
        }
    }
    public bool IsFacingRight()
    {
        return _isFacingRight;
    }

    public bool IsPressingDown()
    {
        return InputAction.IsPressed();
    }

    public void DisableCollision()
    {
        StartCoroutine(DisableCollisionRoutine());
    }
    private IEnumerator DisableCollisionRoutine()
    {
        BoxCollider2D platformColider = _currentOneWayPlatform.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(playerCollider, platformColider);
        yield return new WaitForSeconds(0.25f);

        Physics2D.IgnoreCollision(playerCollider, platformColider, false);
    }

    public void EnterLadder()
    {
        _isInLadder = true;
    }

    public void LeaveLadder()
    {
        _isInLadder = false;
    }

    public void SetRespawnPosition(Vector2 position)
    {
        _respawnPosition = position;
    }

    // private void Jump()
    // {
    //     _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, RespawnJumpSpeed);
    //     //rigidbody.velocity = new Vector2(0, respawnJumpSpeed);
    // }

    public void Die()
    {
        _healthAmount = MaxHealthAmount;
        HealthBar.fillAmount = _healthAmount / 100f;
        _isDead = true;
        _collider.enabled = false;
        Notify(new PlayerDeathEvent());
        StartCoroutine(Respawn());
    }

    public void Kill(IKillable killer)
    {
        if (_isBashing)
        {
            killer.Kill();
        }
        else
        {
            Die();
        }
    }

    public void TakeDamage(float damage, IKillable killer)
    {
        if (_isBashing)
        {
            killer.Kill();
        }
        else
        {
            TakeDamage(damage);
        }
    }

    public void TakeDamage(float damage)
    {
        //Debug.Log("It should take damage: " + damage);
        _healthAmount -= damage;
        _healthAmount = Mathf.Max(_healthAmount, 0);
        HealthBar.fillAmount = _healthAmount / 100f;
        if (_healthAmount <= 0)
        {
            Die();
        }
    }

    public void Heal(float healingAmount)
    {
        _healthAmount += healingAmount;
        _healthAmount = Mathf.Clamp(_healthAmount, 0, 100);
        HealthBar.fillAmount = _healthAmount / 100f;
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(0.4f);
        transform.position = (Vector2)_respawnPosition;
        _collider.enabled = true;
        _isDead = false;
        SetFacingRight();
        stateContext.ChangeState(OnSurfaceState);

        Jump(RespawnJumpSpeed);
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCapsule(_rigidbody.position, new Vector2(2.4f, 2.4f), CapsuleDirection2D.Horizontal, 0, GroundLayer);
    }

    public bool IsOnPlatform()
    {
        return Physics2D.OverlapCapsule(_rigidbody.position, new Vector2(2.4f, 2.4f), CapsuleDirection2D.Horizontal, 0, PlatformLayer);
    }

    public GameObject GetCurrentPlatform()
    {
        return Physics2D.OverlapCapsule(_rigidbody.position, new Vector2(2.4f, 2.4f), CapsuleDirection2D.Horizontal, 0, PlatformLayer).GameObject();
    }

    public bool IsWalled()
    {
        return Physics2D.OverlapCircle(WallCheckRight.position, 0.5f, WallLayer)
        || Physics2D.OverlapCircle(WallCheckLeft.position, 0.5f, WallLayer);
    }

    public void LearnDoubleJump()
    {
        hasLearnedDoubleJump = true;
    }

    public bool CanDoubleJump()
    {
        return hasLearnedDoubleJump;
    }

    public void Jump(float power)
    {
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, power);
        _isOnAir = true;
    }
    public float GetBashingSpeed()
    {
        return BashingSpeed;
    }

    public float GetBashingTime()
    {
        return BashingTime;
    }

    public void Notify(EndOfLevelEvent notification)
    {
        stateContext.ChangeState(new LevelFinishedState());
    }

    public void Add(IListener<PlayerDeathEvent> listener)
    {
        _listeners.Add(listener);
    }

    public void Detach(IListener<PlayerDeathEvent> listener)
    {
        _listeners.Remove(listener);
    }

    public void Notify(PlayerDeathEvent notification)
    {
        foreach (IListener<PlayerDeathEvent> listener in _listeners)
        {
            listener.Notify(notification);
        }
    }

    public void Notify(HealthItemCollectedEvent notification)
    {
        Heal(notification.HealthPoints);
    }
}

