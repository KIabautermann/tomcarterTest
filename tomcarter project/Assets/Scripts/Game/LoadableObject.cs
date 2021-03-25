using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class LoadableObject : MonoBehaviour
{
    
    // Start is called before the first frame update
    public virtual void Start()
    {
        StartCoroutine(WaitUntilLoad());
        SaveLoadController.OnSaveCalled += SaveToGameState;
    }

    private IEnumerator WaitUntilLoad() 
    {
        GameState gameState = SaveLoadController.CurrentGame();
        while (gameState == null) 
        {
            yield return null;
            gameState = SaveLoadController.CurrentGame();
        }
        LoadFromSavedGameState(gameState);
    }
    
    protected abstract void LoadFromSavedGameState(GameState gameState);
    protected abstract void SaveToGameState(object sender, SaveLoadController.OnSaveCalledEventArgs onSaveArguments);
   
}
