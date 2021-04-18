using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private State<StateMachine> _currentState;
    private ComponentCache<State<StateMachine>> allStates;
    void Start()
    {
        var allComponents = GetComponents<State<StateMachine>>();

        allStates = new ComponentCache<State<StateMachine>>(allComponents);
        
        foreach(var state in allStates.GetAllInstances())
        {
            state.Init(this);
        }
        ChangeState<PlayerState>();
    }
    public void ChangeState<T>() where T : State<StateMachine>
    {
        if (!allStates.GetInstance<T>(out State<StateMachine> newState)) newState = null;
              
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

    public bool IsCurrentState<T>(State<T> state) where T : StateMachine
    {
        return state == _currentState;
    }
    

   
}
