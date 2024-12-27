using UnityEngine;
using UnityEngine.InputSystem;

public class OnAirState : IState
{
    /**
     It is entered after jumping or after leaving another state and not being grounded nor bashing nor on stairs.
     It can transition to grounded, bashing or on stairs.
     It should count the amount of times the user has jumped and allow only one jump on air.
     **/

    private Player _player;
    private StateContext _context;

    // internal attributes  
    private bool _hasJumped;
    private bool _canDoubleJump;
    private float _movementSpeed;
    private Vector2 movement;


    public OnAirState(Player player)
    {
        _player = player;
    }

    public void CheckConditions()
    {
        if (_player.IsOnPlatform())
        {
            _context.ChangeState(_player.OnPlatformState);
        }
        else if (_player.IsGrounded() || _player.IsWalled())
        {
            _context.ChangeState(_player.OnSurfaceState);
        }
        else if (_player.IsBButtonPressed)
        {
            _context.ChangeState(_player.BashingState);
        }
        else if (_player.IsInLadder)
        {
            _context.ChangeState(_player.OnStairsState);
        }
    }

    public void EnterState()
    {
        Debug.Log("Entering OnAirState");
        _hasJumped = false;
        _canDoubleJump = _player.CanDoubleJump();
        _movementSpeed = _player.GetMovementSpeed();
    }

    public void ExitState()
    {
        Debug.Log("Exiting OnAirState");
        _hasJumped = false;
    }

    public void RunPhysicsLogic()
    {
        if (_player.IsAButtonPressed && _canDoubleJump && !_hasJumped)
        {
            _player.Rigidbody.velocity = new Vector2(_player.Rigidbody.velocity.x, _player.GetJumpPower());
            _hasJumped = true;
        }
        float xMovement = movement.x * _movementSpeed;
        float yMovement = movement.y * _movementSpeed;
        _player.Rigidbody.velocity = new Vector2(xMovement, _player.Rigidbody.velocity.y + yMovement);
        //Debug.Log("RigibBodyVelocity: "+ _player.Rigidbody.velocity);
    }

    public void RunUpdateLogic()
    {
        // it allows the player to move laterally and downwards
        movement = new Vector2(_player.LeftStickDirection.x, Mathf.Max(0, _player.LeftStickDirection.y));
    }

    public void SetContext(StateContext context)
    {
        _context = context;
    }
}