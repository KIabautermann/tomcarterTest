using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using UnityEngine.SceneManagement;

public class SaveLoadController : MonoBehaviour
{
    private void Awake() {
        LoadGame();
    }
    
    public static event EventHandler<OnSaveCalledEventArgs> OnSaveCalled;
    public class OnSaveCalledEventArgs : EventArgs {
        public GameState gameState;
    }
 
    private static string FILE_NAME = "/savedGame.gd";
    public void SaveGame()
    {
        Debug.Log("Saving game file");
        FileStream file = File.Create(Application.persistentDataPath + FILE_NAME);
        BinaryFormatter bf = new BinaryFormatter();
        OnSaveCalled?.Invoke(this, new OnSaveCalledEventArgs { gameState = _currentGame});
        bf.Serialize(file, _currentGame);
        file.Close();
    }

    public void ResetGame()
    {
        Debug.Log("Resetting game file");
        if (File.Exists(Application.persistentDataPath + FILE_NAME)) {
            File.Delete(Application.persistentDataPath + FILE_NAME);
        }
    }
    private void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + FILE_NAME))
        {
            Debug.Log($"Found save file in {Application.persistentDataPath + FILE_NAME}");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + FILE_NAME, FileMode.Open);
            _currentGame = (GameState)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            Debug.Log("No save file");
            _currentGame = new GameState();
        }

    }

    // Deberia ser realmente algo estatico esto?
    public static GameState CurrentGame() 
    {
        return _currentGame;
    }

    private static GameState _currentGame;
}
