using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;

public class PlayerStateMachine : MonoBehaviour
{
    protected State<PlayerStateMachine> _currentState;
    
    [SerializeField]
    private TextMeshProUGUI _currentStateDisplay;
    public PlayerData stats;
    private PlayerAbilitySystem abilitySystem;    
    private ComponentCache<State<PlayerStateMachine>> allStates; 

    public virtual void Start()
    {
        abilitySystem = GetComponent<PlayerAbilitySystem>();
        abilitySystem.OnAbilityUnlocked += AbilityUnlocked_Handler;

        var allComponents = abilitySystem.GetAvailableStates().Select(t => this.gameObject.AddComponent(t) as State<PlayerStateMachine>).ToList();
        
        foreach(var state in allComponents)
        {
            state.Init(this);
        }
        allStates = new ComponentCache<State<PlayerStateMachine>>(allComponents);
        
        ChangeState<PlayerIdleState>();
    }
    public void ChangeState<T>() where T : State<PlayerStateMachine>
    {
        if (!allStates.GetInstance(typeof(T), out State<PlayerStateMachine> newState)) newState = null;

        if(_currentState != null)
        {
            if(newState == null || newState.OnCoolDown()) 
            {
                Debug.LogWarning($"Can't Transition to {typeof(T).ToString()}");
                return;
            }

           if (_currentState.isExiting) return;
            _currentState.TriggerTransitionOut();
        }
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

    public void AbilityUnlocked_Handler(object sender, PlayerAbilitySystem.AbiltyUnlockedEventArgs args)
    {
        foreach (Type t in args.added)
        {
            var instance = this.gameObject.AddComponent(t) as State<PlayerStateMachine>;
            instance.Init(this);
            allStates.AddInstance(instance);
        }
        foreach (Type t in args.removed)
        {
            List<State<PlayerStateMachine>> removedInstances = allStates.RemoveInstance(t);
            removedInstances.ForEach(s => Destroy(s));
        }
    }
   
}
