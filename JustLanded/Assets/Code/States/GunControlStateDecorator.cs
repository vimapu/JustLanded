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
        // TODO: implement
    }

    public void EnterState()
    {
        // TODO: implement
    }

    public void ExitState()
    {
        // TODO: implement
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