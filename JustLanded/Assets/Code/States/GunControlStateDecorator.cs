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
        throw new System.NotImplementedException();
    }

    public void EnterState()
    {
        throw new System.NotImplementedException();
    }

    public void ExitState()
    {
        throw new System.NotImplementedException();
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
        throw new System.NotImplementedException();
    }
}