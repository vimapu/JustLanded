using UnityEngine;

public class BashingState : IState
{

    private Player _player;
    private StateContext _context;

    private float _bashingTime;
    private float _bashStartTime;
    private float _bashingSpeed;



    public BashingState(Player player)
    {
        _player = player;
        _bashingTime = _player.GetBashingTime();
        _bashingSpeed = _player.GetBashingSpeed();
    }

    /**
 It can be entered from any other state and it can transition to grounded, air or bashing.
 The player cannot die when colliding with a triangled enemy. It is transitioned from when the time is up. 
 **/
    public void CheckConditions()
    {
        if (!IsBashing())
        {
            if (_player.IsInLadder)
            {
                _context.ChangeState(_player.OnStairsState);
            }
            else if (_player.IsGrounded() || _player.IsWalled())
            {
                _context.ChangeState(_player.OnSurfaceState);
            }
            else if (_player.IsOnPlatform())
            {
                _context.ChangeState(_player.OnPlatformState);
            }
            else
            {
                _context.ChangeState(_player.OnAirState);
            }
        }
    }

    public void EnterState()
    {
        _bashStartTime = Time.time;
        _player.StartBash();
        Debug.Log("Entering bashing state");
    }

    public void ExitState()
    {
        _player.FinishBash();
        Debug.Log("Exiting BashingState");
    }

    public void RunPhysicsLogic()
    {
        var speed = _player.IsFacingRight() ? _bashingSpeed : -_bashingSpeed;
        _player.Rigidbody.velocity = new Vector2(speed, _player.Rigidbody.velocity.y);
    }

    public void RunUpdateLogic()
    {
        // TODO: implement if anything
    }

    public void SetContext(StateContext context)
    {
        _context = context;
    }

    private bool IsBashing()
    {
        //Debug.Log("basing time " + (Time.time - _bashStartTime));
        return (Time.time - _bashStartTime) < _bashingTime;
    }
}