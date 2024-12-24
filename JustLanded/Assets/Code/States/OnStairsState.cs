using UnityEngine;

public class OnStairsState : IState
{
    /**
    When it is entered, it should suspend gravity and start applying movement in all directions from input action. 
    It can transition to grounded, air or bashing.
    **/

    private Player _player;
    private StateContext _context;

    public OnStairsState(Player player)
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