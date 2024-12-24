using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private StateContext stateContext;
    // states' declarations
    private BashingState bashingState;
    private OnAirState onAirState;
    private OnStairsState onStairsState;
    private OnSurfaceState onSurfaceState;

    // Start is called before the first frame update
    void Start()
    {
        onAirState = new OnAirState(this);
        onStairsState = new OnStairsState(this);
        onSurfaceState = new OnSurfaceState(this);
        bashingState = new BashingState(this);
        stateContext = new StateContext(onAirState);
    }

    // Update is called once per frame
    void Update()
    {
        stateContext.RunUpdateLogic();
    }

    void FixedUpdate() {
        stateContext.RunPhysicsLogic();
    }
}
