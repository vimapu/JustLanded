using UnityEngine;

public class BashingState : IState
{

    private Player _player;
    private StateContext _context;
    public BashingState(Player player)
    {
        _player = player;
    }

    /**
 It can be entered from any other state and it can transition to grounded, air or bashing.
 The player cannot die when colliding with a triangled enemy. It is transitioned from when the time is up. 
 **/
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

    public void OnCollisionEnter2D(Collision2D collision)
    {
        throw new System.NotImplementedException();
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        throw new System.NotImplementedException();
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        throw new System.NotImplementedException();
    }

    public void OnTriggerExit2D(Collider2D collider)
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
        _context = context;
    }
}