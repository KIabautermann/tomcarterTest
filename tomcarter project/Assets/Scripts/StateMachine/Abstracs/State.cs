using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class State <T> : MonoBehaviour where T : MonoBehaviour
{
    protected T _target;
    public bool isExiting;
    public UnityEvent onTransitionIn;
    
    // Se disparan eventos per logic update? Son una vez por frame si el current state no es nulo (nunca deberia)
    public UnityEvent onLogicUpdate;
    public UnityEvent onPhysicsUpdate;
    public UnityEvent onTransitionOut;
    public string stateName;

    protected float startTime;

    public virtual void Init(T target)
    {
        _target = target;
    }
   
    public void TriggerTransitionIn()
    {
        startTime = Time.time;
        DoChecks();
        DoTransitionIn();
        onTransitionIn.Invoke();  
        isExiting = false;
    }
    protected abstract void DoTransitionIn();

    public void TriggerLogicUpdate()
    {
        DoLogicUpdate();
        TransitionChecks();
        onLogicUpdate.Invoke();
    }
    protected abstract void DoLogicUpdate();

    public void TriggerPhysicsUpdate()
    {
        DoChecks();
        DoPhysicsUpdate();
        onPhysicsUpdate.Invoke();
    }
    protected abstract void DoPhysicsUpdate();

    public void TriggerTransitionOut()
    {
        DoTransitionOut();
        isExiting = true;
        onTransitionOut.Invoke();
    }
    protected abstract void DoTransitionOut();

    protected abstract void DoChecks();

    protected abstract void TransitionChecks();

    
}
