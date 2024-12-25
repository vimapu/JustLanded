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


    public OnAirState(Player player)
    {
        _player = player;
    }

    public void CheckConditions()
    {
        if (_player.IsGrounded() || _player.IsWalled())
        {
            _context.ChangeState(_player.OnSurfaceState);
        }
        else if (Gamepad.current.bButton.IsPressed())
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
        _hasJumped = false;
        _canDoubleJump = _player.CanDoubleJump();
    }

    public void ExitState()
    {
        _player.IsJumping = false;
        _hasJumped = false;
    }



    public void RunPhysicsLogic()
    {
        throw new System.NotImplementedException();
    }

    public void RunUpdateLogic()
    {
        throw new System.NotImplementedException();
    }

    public void SetContext(StateContext context)
    {
        _context = context;
    }
}