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
    private bool _hasJumped = false;
    private bool _canDoubleJump;
    private float _movementSpeed;
    private Vector2 movement;
    private bool _hasJumpButtonBeenReleased = false;
    private bool _wasPressed = true;


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
    }

    public void EnterState()
    {
        //Debug.Log("Entering OnAirState");
        _hasJumped = false;
        _canDoubleJump = _player.CanDoubleJump();
        _movementSpeed = _player.GetMovementSpeed();
        _player.IsOnAir = true;
        _hasJumpButtonBeenReleased = false;
    }

    public void ExitState()
    {
        //Debug.Log("Exiting OnAirState");
        //Debug.Log("was pressed " + _wasPressed + " can double jump: " + _canDoubleJump + " has been released " + _hasJumpButtonBeenReleased + " has jumped " + _hasJumped);
        _hasJumped = false;
        _player.IsOnAir = false;
        _hasJumpButtonBeenReleased = false;
    }

    public void RunPhysicsLogic()
    {
        if (_player.IsAButtonPressed && _canDoubleJump && _hasJumpButtonBeenReleased && !_hasJumped)
        {
            //Debug.Log("Is double jumping");
            _player.Jump(_player.GetJumpPower());
            _hasJumped = true;
        }
        else
        {
            float xMovement = movement.x * _movementSpeed;
            float yMovement = movement.y * _movementSpeed;
            _player.Rigidbody.velocity = new Vector2(xMovement, _player.Rigidbody.velocity.y + yMovement);
        }
    }

    public void RunUpdateLogic()
    {
        // it allows the player to move laterally and downwards
        movement = new Vector2(_player.LeftStickDirection.x, Mathf.Min(0, _player.LeftStickDirection.y));
        if (_wasPressed && !_player.IsAButtonPressed)
        {
            _hasJumpButtonBeenReleased = true;
        }
        _wasPressed = _player.IsAButtonPressed;
    }

    public void SetContext(StateContext context)
    {
        _context = context;
    }
}