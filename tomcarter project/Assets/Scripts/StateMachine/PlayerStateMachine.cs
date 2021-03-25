using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    private State<PlayerStateMachine> _currentState;
    [SerializeField]
    public PlayerData stats;
    void Start()
    {
        var allStates = GetComponents<State<PlayerStateMachine>>();
        foreach(var state in allStates)
        {
            state.Init(this);
        }
        ChangeState<PlayerGroundedState>();
    }
    public void ChangeState<T>() where T : State<PlayerStateMachine>
    {
        var newState = GetComponent<T>();
        if(_currentState != null && newState != null)
        {
            _currentState.TriggerTransitionOut();
        }
        if(newState != null)
        {
            _currentState = newState;
            _currentState.TriggerTransitionIn();
        }
        else
        {
            Debug.LogWarning("Can't Transition");
        }       
    }
    private void Update() 
    {
        if(_currentState != null)
        {
            _currentState.TriggerLogicUpdate();
        }
    }
    private void FixedUpdate() 
    {
        if(_currentState != null)
        {
            _currentState.TriggerPhysicsUpdate();
        }
    }
}
