using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;

public class PlayerStateMachine : MonoBehaviour
{
    private State<PlayerStateMachine> _currentState;
    [SerializeField]
    private TextMeshProUGUI _currentStateDisplay;
    public PlayerData stats;
    private ComponentCache<State<PlayerStateMachine>> allStates;
    void Start()
    {
        var allComponents = GetComponents<State<PlayerStateMachine>>();

        allStates = new ComponentCache<State<PlayerStateMachine>>(allComponents);
        
        foreach(var state in allStates.GetAllInstances())
        {
            state.Init(this);
        }
        ChangeState<PlayerIdleState>();
    }
    public void ChangeState<T>() where T : State<PlayerStateMachine>
    {
        if (!allStates.GetInstance<T>(out State<PlayerStateMachine> newState)) newState = null;
              
        if(_currentState != null)
        {
            if(newState == null || newState.OnCoolDown()) 
            {
                Debug.LogWarning("Can't Transition");
                return;
            }

            if (_currentState.isExiting) return;
            _currentState.TriggerTransitionOut();
        }

        //Debug.Log($"Transitioned to: {typeof(T).ToString()}");
        _currentState = newState;
        _currentStateDisplay.text = _currentState.stateName;
        _currentState.TriggerTransitionIn();
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

    public bool IsCurrentState(State<PlayerStateMachine> state) 
    {
        return state == _currentState;
    }
}
