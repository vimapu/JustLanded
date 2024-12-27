using UnityEngine;

public class GunControlStateDecorator : IState
{
    private IState _decoratedState;

    public GunControlStateDecorator(IState state)
    {
        _decoratedState = state;
    }

    public void CheckConditions()
    {
        _decoratedState.CheckConditions();
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
        _decoratedState.RunPhysicsLogic();
    }

    public void RunUpdateLogic()
    {
        _decoratedState.RunUpdateLogic();
    }

    public void SetContext(StateContext context)
    {
        _decoratedState.SetContext(context);
    }
}