using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class PoolableObject : MonoBehaviour
{
    public abstract void ResetSceneReferences();
    public void Dispose() 
    {
        DisposalRequested?.Invoke(this, new EventArgs());
        ResetSceneReferences();
    }
    public void OnCollected() 
    {
        PoolCollected?.Invoke();
    }
    public event EventHandler DisposalRequested;
    public Action PoolCollected;
}
