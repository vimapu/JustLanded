using UnityEngine;
using UnityEngine.InputSystem;

public class LevelFinishedState : IState
{

    private StateContext _context;

    public void CheckConditions()
    {
        // nothing
    }

    public void EnterState()
    {
        Debug.Log("Entering level finished state");
        // nothing
    }

    public void ExitState()
    {
        // nothing
    }

    public void RunPhysicsLogic()
    {
        // nothing
    }

    public void RunUpdateLogic()
    {
        // nothing
    }

    public void SetContext(StateContext context)
    {
        _context = context;
    }
}