using UnityEngine;

public class BaseStateDecorator : IState
{
    private IState _decoratedState;
    private Player _player;
    private StateContext _context;


    private Vector2 _direction;

    public BaseStateDecorator(Player player, IState state)
    {
        _decoratedState = state;
        _player = player;
    }

    public void CheckConditions()
    {
        if (_player.IsBButtonPressed && !_decoratedState.Equals(_player.BashingState))
        {
            _context.ChangeState(_player.BashingState);
        }
        else if (_player.IsInLadder && !_decoratedState.Equals(_player.OnStairsState))
        {
            _context.ChangeState(_player.OnStairsState);
        }
        else
        {
            _decoratedState.CheckConditions();
        }
    }

    public void EnterState()
    {
        _decoratedState.EnterState();
    }

    public void ExitState()
    {
        _decoratedState.ExitState();
    }

    public void RunPhysicsLogic()
    {
        if (_player.IsGunActive())
        {
            if (_player.IsLeftStickPressed)
            {
                _player.UpdateGunRotation();
            }
            if (_player.IsRightTriggerPressed)
            {
                _player.Shoot();
            }
        }
        _decoratedState.RunPhysicsLogic();
    }

    public void RunUpdateLogic()
    {
        _direction = _player.LeftStickDirection;
        if ((_player.IsFacingRight() && _direction.x < 0f) || (!_player.IsFacingRight() && _direction.x > 0f))
        {
            _player.Flip();
        }
        _decoratedState.RunUpdateLogic();
    }

    public void SetContext(StateContext context)
    {
        _context = context;
        _decoratedState.SetContext(context);
    }
}