using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class PoolableObject : MonoBehaviour
{
    // Este metodo es la implementacion de cada objeto pooleable para que sepa como desreferenciarse de lo que necesite
    // Esta pensado para cuando un Borrower voluntariamente quiera devolver un game object pooleado, y este entonces deberia
    // idealmente desacoplarse de cualquier referencia a la escena. Tambien se llama desde el Pooler para cuando hay
    // un cambio de escena y los objetos, a pesar de reciclar sus instancias, deben resettearse
    public abstract void ResetSceneReferences();

    // Dispose expone la logica para que un Borrower pueda devolver voluntariamente un objeto. Esto le envia un evento
    // al pooler para que sepa desreferenciarlo del diccionario de <escena, gameObject> y desactivarlo
    public void Dispose() 
    {
        DisposalRequested?.Invoke(this, new EventArgs());
    }

    // OnDispose esta expuesto para que el Object pooler pueda exigir recuperarlo dado que quiere ahora prestarlo a otro
    // Borrower. Todo objeto desencolado va a llamar a este metodo. Esta mas que nada para agregar que el borrower original sea consciente 
    // de que no tiene mas acceso al game object prestado. 
    public void OnDispose()
    {
        PoolCollected?.Invoke();
        ResetSceneReferences();
        PoolCollected = null;
    }

    public virtual void DisposeAnimation()
    {

    }
    
    // Event Handler al que el Pooler se subscribe para manejar los pedidos de un Borrower para liberar un recurso
    public event EventHandler DisposalRequested;
    
    // Action para que el Borrower se entere de que un recurso que pidio prestado, se le quito
    public Action PoolCollected;

    protected virtual void Start() {
        this.gameObject.SetActive(false);
    }
}
