using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LoadableObject : MonoBehaviour
{
    // Start is called before the first frame update
    public virtual void Start()
    {
        StartCoroutine(WaitUntilLoad());
        SaveLoad.OnSaveCalled += SaveToGameState;
    }

    private IEnumerator WaitUntilLoad() 
    {
        GameState gameState = SaveLoad.CurrentGame();
        while (gameState == null) 
        {
            yield return null;
            gameState = SaveLoad.CurrentGame();
        }
        LoadFromSavedGameState(gameState);
    }
    
    protected abstract void LoadFromSavedGameState(GameState gameState);
    protected abstract void SaveToGameState(object sender, SaveLoad.OnSaveCalledEventArgs onSaveArguments);
   
}
