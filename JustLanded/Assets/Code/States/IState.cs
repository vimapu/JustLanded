

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
    void OnCollisionEnter2D(Collision2D collision);
    void OnCollisionExit2D(Collision2D collision);
    void OnTriggerEnter2D(Collider2D collider);
    void OnTriggerExit2D(Collider2D collider);
}
