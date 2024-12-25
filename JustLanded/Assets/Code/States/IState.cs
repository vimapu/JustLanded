

using UnityEngine;

public interface IState
{

    void SetContext(StateContext context);
    void EnterState();
    void ExitState();
    void RunUpdateLogic();

    void RunPhysicsLogic();

    // it checks conditions and moves to the next state
    void CheckConditions();
    
}
