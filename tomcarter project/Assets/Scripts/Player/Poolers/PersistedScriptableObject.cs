using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public abstract class PersistedScriptableObject : ScriptableObject
{
    private void Awake()
    {
        OnBegin();
    }
    private void OnBegin() 
    {
        DontDestroyOnLoad(this);
        OnBeginImpl();
        IsLoaded = true;
       
    }
    private void OnEnd()
    {
        OnEndImpl();
        IsLoaded = false;
    }

    protected abstract void OnBeginImpl();
    protected abstract void OnEndImpl();

    public bool IsLoaded { get; private set; }
    
    #if UNITY_EDITOR
        protected void OnEnable()
        {
        EditorApplication.playModeStateChanged += OnPlayStateChange;
        }
 
        protected void OnDisable()
        {
        EditorApplication.playModeStateChanged -= OnPlayStateChange;
        }
 
        void OnPlayStateChange(PlayModeStateChange state)
        {
        if (state == PlayModeStateChange.EnteredPlayMode)
            {
                OnBegin();
            }
            else if(state == PlayModeStateChange.ExitingPlayMode)
            {
                OnEnd();
            }
        }
    #else
        protected void OnEnable()
        {
            OnBegin();
        }
 
        protected void OnDisable()
        {
            OnEnd();
        }
    #endif
}
