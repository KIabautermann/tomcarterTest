using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class State <T> : MonoBehaviour where T : MonoBehaviour
{
    protected T _target;
    public UnityEvent onTransitionIn;
    public UnityEvent onLogicUpdate;
    public UnityEvent onPhysicsUpdate;
    public UnityEvent onTransitionOut;

    public virtual void Init(T target)
    {
        _target = target;
    }
   
    public void TriggerTransitionIn()
    {
        DoChecks();
        DoTransitionIn();
        onTransitionIn.Invoke();   
    }
    protected abstract void DoTransitionIn();

    public void TriggerLogicUpdate()
    {
        DoLogicUpdate();
        TransitionChecks();
        onTransitionOut.Invoke();
    }
    protected abstract void DoLogicUpdate();

    public void TriggerPhysicsUpdate()
    {
        DoChecks();
        DoPhysicsUpdate();
        onTransitionOut.Invoke();
    }
    protected abstract void DoPhysicsUpdate();

    public void TriggerTransitionOut()
    {
        DoTransitionOut();
        onLogicUpdate.Invoke();
    }
    protected abstract void DoTransitionOut();

    protected abstract void DoChecks();

    protected abstract void TransitionChecks();

    
}
