using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class State <T> : MonoBehaviour where T : MonoBehaviour
{
    protected T _target;
    private bool isExiting;
    
    // Vamos a tener que refactorizar esto a un sistema publico de Eventos, ya que estamos en plan de que los States se agreguen dinamicamente
    // No tengo buenas ideas por ahora. Porque el tema de hacerlo publico-estatico es que perdemos la posibilidad de hacer algo que escuche el OnTransitionIn
    // de UNA instancia en particular del State. Por ejemplo, multiples enemigos pueden tener multiples instancias del mismo estado, entonces
    // no pueden estar todas escuchando el mismo evento
    // A menos que parte del parametro sea el propio this o parent del game object, cosa de quien se subscribio al evento solo lo escucha si el parametro
    // que viene es la instancia que SI quiere escuchar. Onda, que un subscriptor al evento solo actue si comparte el parent del game object con el del State que notifico
    public UnityEvent onTransitionIn = new UnityEvent();
    
    // Se disparan eventos per logic update? Son una vez por frame si el current state no es nulo (nunca deberia)
    public UnityEvent onLogicUpdate = new UnityEvent();
    public UnityEvent onPhysicsUpdate = new UnityEvent();
    public UnityEvent onTransitionOut = new UnityEvent();
    public string animationTrigger  { get; protected set; }
    public int stateIndex { get; protected set; }
    public int animationIndex  { get; protected set; }
    public bool asynchronous { get; protected set; }
    protected float coolDown;
    protected float counter;
    protected float endTime;
    protected bool isLocked = false;
    protected bool endByAnimation;

    public virtual void Init(T target)
    {
       coolDown = 0;
       endTime = Time.time;
        _target = target;
    }
   
    public void TriggerTransitionIn()
    {
        counter = 0;
        animationIndex = 0;
        DoChecks();
        DoTransitionIn();
        onTransitionIn?.Invoke();  
        isExiting = false;
        endByAnimation = false;
    }
    protected abstract void DoTransitionIn();

    public void TriggerLogicUpdate()
    {
        DoLogicUpdate();
        DoChecks();
        TransitionChecks();
        onLogicUpdate?.Invoke();
        counter+= Time.deltaTime;
    }
    protected abstract void DoLogicUpdate();

    public void TriggerPhysicsUpdate()
    {      
        DoPhysicsUpdate();
        onPhysicsUpdate?.Invoke();
    }
    protected abstract void DoPhysicsUpdate();

    public void TriggerTransitionOut()
    {
        DoTransitionOut();
        isExiting = true;
        onTransitionOut?.Invoke();
        endTime = Time.time;
    }
    protected abstract void DoTransitionOut();

    protected abstract void DoChecks();

    protected abstract void TransitionChecks();

    protected abstract void OnDestroyHandler();

    public void AnimationEnded()
    {
        endByAnimation = true;
    }


    public bool OnCoolDown() => Time.time < endTime + coolDown;
    public bool IsLocked() => isLocked;
    public void ToggleLock(bool isLock) => isLocked = isLock;
    public void ChangeAnimationState(int index) => animationIndex = index;
    private void OnDestroy() {
        OnDestroyHandler();
    }

}
