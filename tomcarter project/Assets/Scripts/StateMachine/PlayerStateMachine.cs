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
    private ComponentCache<PlayerState> allStates; 

    public virtual void Start()
    {
        abilitySystem = GetComponent<PlayerAbilitySystem>();
        abilitySystem.OnAbilityUnlocked += AbilityUnlocked_Handler;

        allStates = abilitySystem.GetAvailableStates();
        
        foreach(var state in allStates.GetAllInstances())
        {
            state.Init(this);
        }
        
        ChangeState<PlayerIdleState>();
    }
    public void ChangeState<T>() where T : PlayerState
    {
        if (!allStates.GetInstance(typeof(T), out PlayerState newState)) newState = null;

        if(_currentState != null)
        {
            if(newState == null || newState.OnCoolDown() || newState.IsLocked()) 
            {
                //Debug.LogWarning($"Can't Transition to {typeof(T).ToString()}");
                if(newState.IsLocked()){
                    Debug.LogWarning("The state is locked");
                }
                else if(newState.OnCoolDown()){
                    Debug.LogWarning("The state is on cooldown");
                }
                
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
            allStates.SetActive(t);
        }
        foreach (Type t in args.removed)
        {
            allStates.SetInactive(t);
        }
    }
   
}
