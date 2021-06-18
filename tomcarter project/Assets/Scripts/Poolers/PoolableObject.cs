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
    // al pooler para que sepa desreferenciarlo del diccionario de <escena, gameObject> y desactivarlo. Esto tambien
    // deberia ser usado por el Pooler cuando sabemos que el Borrower ya tal vez fue destruido (por ejemplo un cambio de escena)
    public void Dispose() 
    {
        DisposalRequested?.Invoke(this, new EventArgs());
        ResetSceneReferences();
        PoolCollected = null;
    }
    
    // Collect esta expuesto para que el Object pooler pueda exigir recuperarlo dado que quiere ahora prestarlo a otro
    // Borrower. Todo objeto desencolado va a llamar a este metodo. Esta mas que nada para agregar que el borrower original sea consciente 
    // de que no tiene mas acceso al game object prestado. 
    public void Collect() 
    {
        PoolCollected?.Invoke();
        Dispose();
    }
    public event EventHandler DisposalRequested;
    public Action PoolCollected;
}
