using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;

public class PlayerStateMachine : MonoBehaviour
{
    public State<PlayerStateMachine> _currentState {get; private set;}
    public bool gravityException { get; private set; }

    public CanvasReference canvasReference;
    public ObjectPooler sporeTrailPooler;
    public PlayerData stats;
    public PlayerAnimationData animations;
    public VisualEffectSpawner vfxSpawn;
    public ProjectileSpawner projectileSpawn;
    private PlayerAbilitySystem abilitySystem;    
    private ComponentCache<PlayerState> allStates;
    private AnimatorController _anim;
    [SerializeField]
    private SpriteMask _mask;


    public virtual void Start()
    {
        abilitySystem = GetComponent<PlayerAbilitySystem>();
        abilitySystem.OnAbilityUnlocked += AbilityUnlocked_Handler;
        _anim = GetComponent<AnimatorController>();
        allStates = abilitySystem.GetAvailableStates();
        
        foreach(var state in allStates.GetAllInstances())
        {
            state.Init(this);
        }
        StartCoroutine(LoadScriptableObjects());
    }
    
    private IEnumerator LoadScriptableObjects() 
    {
        while (!canvasReference.IsLoaded) {
            yield return new WaitForEndOfFrame();
        }
        ChangeState<PlayerIdleState>();
    }
    public void ChangeState<T>() where T : PlayerState
    {
        if (!allStates.GetInstance(typeof(T), out PlayerState newState)) newState = null;

        TextMeshProUGUI stateDisplay = canvasReference.GetTextMeshForGameObject(CanvasElement.TopBanner);

        if(newState != null){
            if(!newState.OnCoolDown() && !newState.IsLocked()){               
                
                if(_currentState!= null){
                    _currentState.TriggerTransitionOut();
                    //Debug.Log(_currentState.animationTrigger + " -> " + newState.animationTrigger);
                }            
                _currentState = newState;
                _currentState.TriggerTransitionIn();   
                stateDisplay.text = _currentState.animationTrigger;
            }           
        }  
        else{
            Debug.Log("There's no state");
        }    
    }

    private void Update() 
    {
        if(_currentState != null) _currentState.TriggerLogicUpdate(); 
    }
    private void FixedUpdate() 
    {
        if(_currentState != null) _currentState.TriggerPhysicsUpdate();

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

    public void SetMaskPosition(Vector3 position, Vector3 offsetDirection)
    {
        _mask.transform.position = position + offsetDirection * _mask.transform.localScale.x/2 ;
    }
    public void SetMaskSize(Vector3 size) => _mask.transform.localScale = size;
    public void SetMaskActive(bool active)
    {
        _mask.gameObject.SetActive(active);
        if (!active)
        {
            _mask.transform.position = transform.position;
        }
    }


    public void QueueAnimation(string clip, bool locked, bool priority){
        _anim.Queue(clip, locked, priority);
    }

    public IEnumerator GravityExceptionTime()
    {
        gravityException = true;
        yield return new WaitForSeconds(stats.postDashTimer);
        gravityException = false;       
        StopCoroutine(GravityExceptionTime());
    }
}
