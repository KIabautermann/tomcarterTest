using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;

public class PlayerStateMachine : MonoBehaviour
{
    private class ComponentCache<T> where T : MonoBehaviour 
    {
        private Dictionary<Type, T> allComponents;
        public ComponentCache() { allComponents = new Dictionary<Type, T>(); }
        public ComponentCache(IEnumerable<T> instances) 
        {
            allComponents = new Dictionary<Type, T>(); 
            instances.ToList().ForEach((T instance) => AddInstance(instance));
        }

        public IEnumerable<T> GetAllInstances()
        {
            return allComponents.Values.Distinct();
        }

        public bool GetInstance<V>(out T instance) where V : T 
        {
            return allComponents.TryGetValue(typeof(V), out instance);
        }

        public void AddInstance(T instance)
        {
            Type getType = instance.GetType();

            while (getType != typeof(T)) 
            {
                allComponents[getType] = instance;
                getType = getType.BaseType;
            }
        }

    }
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
