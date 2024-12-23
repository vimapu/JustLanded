using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    
    void RunUpdateLogic();

    void RunPhysicsLogic();

    // it should return null if it is in the right state
    IState GetOptionalNextState();

}
