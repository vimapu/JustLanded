using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private StateContext stateContext;
    // states' declarations
    private IState _bashingState;
    public IState BashingState { get { return _bashingState; } }
    private IState _onAirState;
    public IState OnAirState { get { return _onAirState; } }
    private IState _onStairsState;
    public IState OnStairsState { get { return _onStairsState; } }
    private IState _onSurfaceState;
    public IState OnSurfaceState { get { return _onSurfaceState; } }


    // Start is called before the first frame update
    void Start()
    {
        _onAirState = new GunControlStateDecorator(new OnAirState(this));
        _onStairsState = new GunControlStateDecorator(new OnStairsState(this));
        _onSurfaceState = new GunControlStateDecorator(new OnSurfaceState(this));
        _bashingState = new GunControlStateDecorator(new BashingState(this));
        stateContext = new StateContext(_onAirState);
    }

    // Update is called once per frame
    void Update()
    {
        stateContext.RunUpdateLogic();
    }

    void FixedUpdate()
    {
        stateContext.RunPhysicsLogic();
    }
}
