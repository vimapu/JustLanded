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

    [Header("Gun parameters")]
    [SerializeField] GameObject Pistol;
    [SerializeField] float SecondBetweenShots;
    [SerializeField] GameObject Bullet;
    [SerializeField] Transform BulletSpawnPoint;


    private GameObject _pistol;

    private Rigidbody2D _rigidbody;

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
}
