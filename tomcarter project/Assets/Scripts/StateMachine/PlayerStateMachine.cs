using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;

public class PlayerStateMachine : MonoBehaviour
{
    protected State<PlayerStateMachine> _currentState;
    protected State<PlayerStateMachine> _currentSubstate;
    
    [SerializeField]
    private TextMeshProUGUI _currentStateDisplay;
    public PlayerData stats;
    private PlayerAbilitySystem abilitySystem;    
    private ComponentCache<PlayerState> allStates; 
    private AnimationController _animations;

    public virtual void Start()
    {
        abilitySystem = GetComponent<PlayerAbilitySystem>();
        abilitySystem.OnAbilityUnlocked += AbilityUnlocked_Handler;

        _currentSubstate = null;

        allStates = abilitySystem.GetAvailableStates();

        _animations = GetComponent<AnimationController>();
        
        foreach(var state in allStates.GetAllInstances())
        {
            state.Init(this);
        }
        
        ChangeState<PlayerIdleState>();
    }
    public void ChangeState<T>() where T : PlayerState
    {
        if (!allStates.GetInstance(typeof(T), out PlayerState newState)) newState = null;

        if(newState != null){
            if(!newState.OnCoolDown() && !newState.IsLocked()){
                if(newState.asynchronous){
                    if(_currentSubstate == null){

                        _currentSubstate = newState;
                        _animations.SetBool(_currentSubstate.animationTrigger,true);
                        _currentStateDisplay.text = _currentState.animationTrigger + (" / ") + _currentSubstate.animationTrigger;
                        _currentSubstate.TriggerTransitionIn();
                    }
                }
                else{
                    if(_currentState!= null){
                        _currentState.TriggerTransitionOut();
                        _animations.SetBool(_currentState.animationTrigger,false);  
                    }    
                    _currentState = newState;
                    _animations.SetBool(_currentState.animationTrigger,true);
                    _currentState.TriggerTransitionIn();   
                    if(_currentSubstate!=null) _currentStateDisplay.text = _currentState.animationTrigger + (" / ") + _currentSubstate.animationTrigger;
                    else _currentStateDisplay.text = _currentState.animationTrigger;
                }
            }           
        }  
        else{
            Debug.Log("There's no state");
        }    
    }

    private void Update() 
    {
        if(_currentState != null) _currentState.TriggerLogicUpdate();

        if(_currentSubstate != null) _currentSubstate.TriggerLogicUpdate();
    }
    private void FixedUpdate() 
    {
        if(_currentState != null) _currentState.TriggerPhysicsUpdate();

        if(_currentSubstate != null)  _currentSubstate.TriggerPhysicsUpdate();
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

    public void removeSubState(){
        _animations.SetBool(_currentSubstate.animationTrigger, false);
        _currentSubstate.TriggerTransitionOut();
        _currentSubstate = null;
    }
   
}
