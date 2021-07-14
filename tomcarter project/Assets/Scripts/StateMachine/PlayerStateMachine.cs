using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;

public class PlayerStateMachine : MonoBehaviour
{
    public State<PlayerStateMachine> _currentState {get; private set;}
    public State<PlayerStateMachine> _currentSubstate {get; private set;}
    
    [SerializeField]
    public CanvasReference canvasReference;
    private TextMeshProUGUI _currentStateDisplay;
    public PlayerData stats;
    private PlayerAbilitySystem abilitySystem;    
    private ComponentCache<PlayerState> allStates;
    private AnimationController _anim;

    private Vector2 currentAnimationIndex;
    private Vector2 lastAnimationIndex;
    private Vector2 currentSubAnimationIndex;
    private Vector2 lastSubAnimationIndex;

    public virtual void Start()
    {
        abilitySystem = GetComponent<PlayerAbilitySystem>();
        abilitySystem.OnAbilityUnlocked += AbilityUnlocked_Handler;
        _anim = GetComponent<AnimationController>();
        _currentSubstate = null;
        
        allStates = abilitySystem.GetAvailableStates();
        
        foreach(var state in allStates.GetAllInstances())
        {
            state.Init(this);
        }
        StartCoroutine(LoadScriptableObjects());
        lastAnimationIndex = (currentAnimationIndex);
    }
    
    private IEnumerator LoadScriptableObjects() 
    {
        while (!canvasReference.IsLoaded) {
            yield return new WaitForEndOfFrame();
        }
        _currentStateDisplay = canvasReference.GetTextMeshForGameObject(CanvasElement.TopBanner);
        ChangeState<PlayerIdleState>();
        _anim.PlayAnimation(_currentState.stateIndex, _currentState.animationIndex);
        currentAnimationIndex.Set(_currentState.stateIndex, _currentState.animationIndex);
    }
    public void ChangeState<T>() where T : PlayerState
    {
        if (!allStates.GetInstance(typeof(T), out PlayerState newState)) newState = null;

        if(newState != null){
            if(!newState.OnCoolDown() && !newState.IsLocked()){
                if(newState.asynchronous){
                    if(_currentSubstate == null){

                        _currentSubstate = newState;
                        _currentStateDisplay.text = _currentState.animationTrigger + (" / ") + _currentSubstate.animationTrigger;
                        _currentSubstate.TriggerTransitionIn();
                    }
                }
                else{
                    if(_currentState!= null){
                        _currentState.TriggerTransitionOut();
                    }    
                    _currentState = newState;
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

    private void LateUpdate() {
        if(_currentState!=null) CheckForAnimation();          
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

    private void CheckForAnimation(){
        currentAnimationIndex.Set(_currentState.stateIndex, _currentState.animationIndex);
        if(currentAnimationIndex != lastAnimationIndex){
            _anim.PlayAnimation(_currentState.stateIndex, _currentState.animationIndex);
            lastAnimationIndex = currentAnimationIndex;
        }
        if(_currentSubstate!=null){
            currentSubAnimationIndex.Set(_currentSubstate.stateIndex, _currentSubstate.animationIndex);
            if(lastSubAnimationIndex != currentSubAnimationIndex){
                _anim.PlayAnimation(_currentSubstate.stateIndex, _currentSubstate.animationIndex);   
                lastSubAnimationIndex = currentSubAnimationIndex;   
            }          
        }
    }

    public void SetAnimationIndex(int newIndex) => _currentState.animationIndex = newIndex;

    public void removeSubState(){
        if(_currentSubstate!=null){
            _currentSubstate.TriggerTransitionOut();
            _currentSubstate = null;
            lastSubAnimationIndex.Set(0,0);
        }       
    }   

    public void EndStateByAnimation(){
        _currentState.AnimationEnded();
    }

    public void EndSubStateByAnimation(){
        if(_currentSubstate!=null){
            _currentSubstate.AnimationEnded();
        }       
    }

    public void PlayAnimation(int x, int y){
        _anim.PlayAnimation(x,y);
    }
}
