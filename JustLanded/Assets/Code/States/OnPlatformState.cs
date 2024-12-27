using UnityEditor.Experimental.GraphView;
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

    public OnPlatformState(Player player)
    {
        _player = player;
        _movementSpeed = _player.GetMovementSpeed();
    }
    public void CheckConditions()
    {
        if (_player.IsBButtonPressed)
        {
            _context.ChangeState(_player.BashingState);
        }
        else if (!_player.IsOnPlatform())
        {
            if (_player.IsOnAir)
            {
                _context.ChangeState(_player.OnAirState);
            }
            else if (_player.IsGrounded() || _player.IsWalled())
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
        Debug.Log("Entering OnPlatformState");
        var currentPlatform = _player.GetCurrentPlatform();
        _currentPlatformRigidBody = currentPlatform.GetComponent<Rigidbody2D>();
        _isOneWayPlatform = currentPlatform.CompareTag("OneWayPlatform");
    }

    public void ExitState()
    {
        Debug.Log("Exiting OnPlatformState");
        _isOneWayPlatform = false;
        _currentPlatformRigidBody = null;
    }

    public void RunPhysicsLogic()
    {
        if (_player.IsAButtonPressed) // jumping
        {
            _player.Jump(_player.GetJumpPower());
        }
        else
        {
            float yMovement = 0f;
            if (_isOneWayPlatform && _movement.y < 0.01f) // going through one way platforms
            {
                yMovement = _movement.y * _movementSpeed;
                _player.DisableCollision();
            }
            _player.Rigidbody.velocity = new Vector2(_movement.x * _movementSpeed, yMovement) + _currentPlatformRigidBody.velocity;
        }
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