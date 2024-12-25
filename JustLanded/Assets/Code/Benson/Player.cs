using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    [Header("Bashing parameters")]
    [SerializeField] float BashingSpeed = 40f;
    [SerializeField] float BashingTime = 0.5f;

    [Header("Movement parameters")]
    [SerializeField] float Speed;
    [SerializeField] InputAction InputAction;
    [SerializeField] private CircleCollider2D playerCollider;

    [Header("Gun parameters")]
    [SerializeField] GameObject Pistol;
    [SerializeField] float SecondBetweenShots;
    [SerializeField] GameObject Bullet;
    [SerializeField] Transform BulletSpawnPoint;

    [Header("Respawning parameters")]
    [SerializeField] float RespawnJumpSpeed = 10f;

    [Header("Jump System")]
    [SerializeField] float JumpPower;
    [SerializeField] float JumpPowerPercentWhenReleased;

    [SerializeField] LayerMask GroundLayer;
    [SerializeField] LayerMask WallLayer;
    [SerializeField] Transform WallCheckRight;
    [SerializeField] Transform WallCheckLeft;



    private GameObject _pistol;

    private Rigidbody2D _rigidbody;
    private Collider2D _collider;

    private StateContext stateContext;
    // states' declarations
    private IState _bashingState;
    public IState BashingState { get { return _bashingState; } }
    private IState _onAirState;
    public IState OnAirState { get { return _onAirState; } }
    private IState _onStairsState;
    public IState OnStairsState { get { return _onStairsState; } }
    private IState _onSurfaceState;
    public IState OnSurfaceState { get { return _onSurfaceState; } }

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
    private float _speed = 8f;
    private float _gravityScale;
    private bool _isInLadder = false;
    public bool IsInLadder { get { return _isInLadder; } }

    // death and respawning attributes
    private bool _isDead = false;
    private Vector2 _respawnPosition;
    // jump attributes
    private bool canDoubleJump = false;
    private bool hasLearnedDoubleJump = false;
    private bool _isJumping = false;
    public bool IsJumping { get { return _isJumping; } set { _isJumping = value; } }

    // gamepad controller attributes
    private bool _isAButtonPressed = false;
    public bool IsAButtonPressed { get { return _isAButtonPressed; } }
    private bool _isBButtonPressed = false;
    public bool IsBButtonPressed { get { return _isBButtonPressed; } }
    private bool _isYButtonPressed = false;
    public bool IsYButtonPressed { get { return _isYButtonPressed; } }
    private Vector2 _leftStickDirection;
    public Vector2 LeftStickDirection { get { return _leftStickDirection; } }


    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _onAirState = new GunControlStateDecorator(new OnAirState(this));
        _onStairsState = new GunControlStateDecorator(new OnStairsState(this));
        _onSurfaceState = new GunControlStateDecorator(new OnSurfaceState(this));
        _bashingState = new GunControlStateDecorator(new BashingState(this));
        stateContext = new StateContext(_onAirState);
        InputAction.Enable();
        _pistol = GameObject.Find("Pistol");
        _pistol.SetActive(false);
        _canClimb = false;
        _gravityScale = GetComponent<Rigidbody2D>().gravityScale;
        _collider = GetComponent<Collider2D>();
        SetRespawnPosition(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        stateContext.RunUpdateLogic();
    }

    void FixedUpdate()
    {
        stateContext.RunPhysicsLogic();
    }

    /* Methods to control the bashing state*/
    void StartBash()
    {
        _isBashing = true;
        _bashStartTime = Time.time;
    }

    void FinishBash()
    {
        _isBashing = false;
    }

    public bool IsBashing()
    {
        return _isBashing && (Time.time - _bashStartTime) < BashingTime;
    }

    public void SetFacingRight()
    {
        if (!_isFacingRight)
        {
            var localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    public void CollectPistol()
    {
        _pistol.SetActive(true);
        _hasPistol = true;
        ActivateGun();
    }

    public bool IsFacingRight()
    {
        return _isFacingRight;
    }

    private void Shoot()
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
    private void UpdateGunRotation()
    {

        var aimAngle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        if (_isFacingRight)
        {
            if (_direction.x >= 0)
            {
                _pistol.transform.rotation = Quaternion.Euler(0, 0, aimAngle);
            }
        }
        else
        {
            if (_direction.x <= 0)
            {
                _pistol.transform.rotation = Quaternion.Euler(0, 0, aimAngle - 180);
            }
        }
    }

    public void Flip()
    {
        _isFlipped = !_isFlipped;
    }

    public void ActivateGun()
    {
        _isGunActive = true;
    }

    public void DeactivateGun()
    {
        _isGunActive = false;
    }

    public bool IsPressingDown()
    {
        return InputAction.IsPressed();
    }

    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformColider = _currentOneWayPlatform.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(playerCollider, platformColider);
        yield return new WaitForSeconds(0.25f);

        Physics2D.IgnoreCollision(playerCollider, platformColider, false);
    }

    public void EnableClimbing()
    {
        _canClimb = true;
        _rigidbody.gravityScale = 0f;
    }

    public void DisableClimbing()
    {
        _canClimb = false;
        _rigidbody.gravityScale = _gravityScale;
    }

    public void SetRespawnPosition(Vector2 position)
    {
        _respawnPosition = position;
    }

    private void Jump()
    {
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, RespawnJumpSpeed);
        //rigidbody.velocity = new Vector2(0, respawnJumpSpeed);
    }

    public void Die()
    {
        _isDead = true;
        _collider.enabled = false;
        StartCoroutine(Respawn());
    }

    public void Kill(IKillable killer)
    {
        if (IsBashing())
        {
            killer.Kill();
        }
        else
        {
            Die();
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(0.4f);
        transform.position = (Vector2)_respawnPosition;
        _collider.enabled = true;
        _isDead = false;
        SetFacingRight();

        Jump();
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCapsule(_rigidbody.position, new Vector2(2.4f, 2.4f), CapsuleDirection2D.Horizontal, 0, GroundLayer);
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
        _isJumping = true; // TODO: see if it can be substituted by isOnAir
    }
}
