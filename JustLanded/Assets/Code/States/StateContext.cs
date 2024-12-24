
using Unity.VisualScripting;

public class StateContext
{
    private IState _state;

    public StateContext(IState initialState)
    {
        _state = initialState;
        _state.EnterState();
        _state.SetContext(this);
    }

    public void RunUpdateLogic()
    {
        _state.CheckConditions();
        _state.RunUpdateLogic();
    }

    public void RunPhysicsLogic()
    {
        _state.RunPhysicsLogic();
    }

    public void ChangeState(IState newState)
    {
        _state.ExitState();
        _state = newState;
        _state.EnterState();
        _state.SetContext(this);
    }


}