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
    private AnimationController animations;

    public virtual void Start()
    {
        abilitySystem = GetComponent<PlayerAbilitySystem>();
        abilitySystem.OnAbilityUnlocked += AbilityUnlocked_Handler;

        allStates = abilitySystem.GetAvailableStates();

        animations = GetComponent<AnimationController>();
        
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
                if(newState == null){
                    Debug.LogWarning("State does not exist");
                }
                else if(newState.IsLocked()){
                    Debug.LogWarning("The state is locked");
                }
                else if(newState.OnCoolDown()){
                    Debug.LogWarning("The state is on cooldown");
                }
                
                return;
            }

            if (_currentState.isExiting) return;
            _currentState.TriggerTransitionOut();
            if(newState != _currentState){
                animations.SetBool(_currentState.animationTrigger,false);
            }  
            else{
                animations.SetTrigger("repeat");
            }        
        }
        _currentState = newState;
        _currentStateDisplay.text = _currentState.animationTrigger;
        _currentState.TriggerTransitionIn();
        animations.SetBool(_currentState.animationTrigger,true);
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
