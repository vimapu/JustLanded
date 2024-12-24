using UnityEngine;

public class OnAirState : IState
{
    /**
     It is entered after jumping or after leaving another state and not being grounded nor bashing nor on stairs.
     It can transition to grounded, bashing or on stairs.
     It should count the amount of times the user has jumped and allow only one jump on air.
     **/

    private Player _player;
    private StateContext _context;

    public OnAirState(Player player)
    {
        _player = player;
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