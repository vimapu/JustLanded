using UnityEngine;

public class OnLadderState : IState
{
    /**
    When it is entered, it should suspend gravity and start applying movement in all directions from input action. 
    It can transition to grounded, air or bashing.
    **/

    private Player _player;
    private StateContext _context;

    // internal attributes
    private float _gravityScale;
    private float _ladderSpeed;
    private Vector2 _movement;

    public OnLadderState(Player player)
    {
        _player = player;
        _gravityScale = _player.Rigidbody.gravityScale;
        _ladderSpeed = _player.GetLadderSpeed();
    }
    public void CheckConditions()
    {
        if (!_player.IsInLadder)
        {
            if (_player.IsGrounded() || _player.IsWalled())
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
        _player.Rigidbody.gravityScale = 0f;
    }

    public void ExitState()
    {
        _player.Rigidbody.gravityScale = _gravityScale;
    }



    public void RunPhysicsLogic()
    {
        _player.Rigidbody.velocity = _movement;
    }

    public void RunUpdateLogic()
    {
        _movement = _player.LeftStickDirection * _ladderSpeed;
    }

    public void SetContext(StateContext context)
    {
        _context = context;
    }
}