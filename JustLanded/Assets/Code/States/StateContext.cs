
using Unity.VisualScripting;

public class StateContext
{
    private IState State;

    public StateContext(IState initialState)
    {
        State = initialState;
    }

    void RunUpdateLogic()
    {
        State.CheckConditions();
        State.RunUpdateLogic();
    }

    void RunPhysicsLogic()
    {
        State.RunPhysicsLogic();
    }

    void ChangeState(IState newState)
    {
        State.ExitState();
        State = newState;
        State.EnterState();
        State.SetContext(this);
    }


}