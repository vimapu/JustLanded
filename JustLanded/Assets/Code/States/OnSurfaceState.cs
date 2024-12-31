

using UnityEngine;

public class OnSurfaceState : IState
{

    private Player _player;
    private StateContext _context;

    private Vector2 _movement;
    private float _movementSpeed;

    public OnSurfaceState(Player player)
    {
        _player = player;
        _movementSpeed = _player.GetMovementSpeed();
    }

    public void CheckConditions()
    {
        if (_player.IsBButtonPressed)
        {
    //        _context.ChangeState(_player.BashingState);
        }
        else if (!_player.IsGrounded() && !_player.IsWalled())
        {
            if (_player.IsOnAir)
            {
                _context.ChangeState(_player.OnAirState);
            }
            else if (_player.IsOnPlatform())
            {
                _context.ChangeState(_player.OnPlatformState);
            }
        }
    }

    public void EnterState()
    {
        // TODO: implement
        //Debug.Log("Entering on surface state");
        _player.IsOnAir = false;

    }

    public void ExitState()
    {
        //Debug.Log("Exiting on surface state");
    }



    public void RunPhysicsLogic()
    {
        if (_player.IsAButtonPressed) // jumping
        {
            _player.Jump(_player.GetJumpPower());
        }
        else
        {
            _player.Rigidbody.velocity = new Vector2(_movement.x * _movementSpeed, _player.Rigidbody.velocity.y);
        }
    }

    public void RunUpdateLogic()
    {
        _movement = new Vector2(_player.LeftStickDirection.x, _player.LeftStickDirection.y);
    }

    public void SetContext(StateContext context)
    {
        _context = context;
    }
}