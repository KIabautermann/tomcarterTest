using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class PlayerBaseState <T> : MonoBehaviour where T : MonoBehaviour
{
    protected T _target;

    public UnityEvent onTransitionIn;
    public UnityEvent onUpdate;
    public UnityEvent onTransitionOut;

    public void Init(T target)
    {
        _target = target;
    }
   
    public void TriggerTransitionIn()
    {
        DoTransitionIn();
        onTransitionIn.Invoke();
    }
    protected abstract void DoTransitionIn();

    public void TriggerUpdate()
    {
        DoUpdate();
        onTransitionOut.Invoke();
    }
    protected abstract void DoUpdate();

    public void TriggerTransitionOut()
    {
        DoTransitionOut();
        onUpdate.Invoke();
    }
    protected abstract void DoTransitionOut();
}
