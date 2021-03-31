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
    private Dictionary<Type, State<PlayerStateMachine>> allStates;
    void Start()
    {
        var allComponents = GetComponents<State<PlayerStateMachine>>();
        
        allStates = allComponents.ToDictionary(s => s.GetType(), s => s);
        foreach(var state in allStates.Values)
        {
            state.Init(this);
        }
        ChangeState<PlayerIdleState>();
    }
    public void ChangeState<T>() where T : State<PlayerStateMachine>
    {
        if (!allStates.TryGetValue(typeof(T), out State<PlayerStateMachine> newState)) newState = null;
        
        if(newState == null) 
        {
            Debug.LogWarning("Can't Transition");
            return;
        }

        if(_currentState != null)
        {
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
}
