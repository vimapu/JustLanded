using UnityEngine;

public class OnPlatformState : IState
{

    private Player _player;
    private StateContext _context;

    // internal attributes
    private Rigidbody2D _currentPlatformRigidBody;
    private bool _isOneWayPlatform;
    private Vector2 _movement;
    private float _movementSpeed;
    private float _jumpSpeed;

    public OnPlatformState(Player player)
    {
        _player = player;
    }
    public void CheckConditions()
    {
        if (_player.IsBButtonPressed)
        {
            _context.ChangeState(_player.BashingState);
        }
        else if (!_player.IsOnPlatform())
        {
            if (_player.IsGrounded() || _player.IsWalled())
            {
                _context.ChangeState(_player.OnSurfaceState);
            }
            else if (_player.IsInLadder)
            {
                _context.ChangeState(_player.OnStairsState);
            }
        }
    }

    public void EnterState()
    {
        var currentPlatform = _player.GetCurrentPlatform();
        _currentPlatformRigidBody = currentPlatform.GetComponent<Rigidbody2D>();
        _isOneWayPlatform = currentPlatform.CompareTag("OneWayPlatform");
    }

    public void ExitState()
    {
        _isOneWayPlatform = false;
        _currentPlatformRigidBody = null;
    }

    public void RunPhysicsLogic()
    {
        float yMovement = 0f;

        if (_player.IsAButtonPressed) // jumping
        {
            yMovement = _player.GetJumpPower();
        }
        else if (_isOneWayPlatform && _movement.y < 0.01f) // going through one way platforms
        {
            yMovement = _movement.y * _movementSpeed;
            _player.DisableCollision();
        }
        _player.Rigidbody.velocity = new Vector2(_movement.x * _movementSpeed, yMovement) + _currentPlatformRigidBody.velocity;
    }

    public void RunUpdateLogic()
    {
        // it allows the player to move laterally and downwards
        _movement = new Vector2(_player.LeftStickDirection.x, Mathf.Max(0, _player.LeftStickDirection.y));
    }

    public void SetContext(StateContext context)
    {
        _context = context;
    }
}