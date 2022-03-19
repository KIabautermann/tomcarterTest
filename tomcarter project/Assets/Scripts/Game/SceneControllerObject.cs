using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Threading.Tasks;
using System;

[CreateAssetMenu(fileName = "SceneController", menuName = "Scene Managers")]
public class SceneControllerObject : PersistedScriptableObject
{
    [SerializeField]
    public List<PersistedScriptableObject> ScriptableObjectsDependencies;
    [SerializeField]
    public int PreloadScene = 0;
    [SerializeField]
    public int DefaultScene = 1;
    private int currSceneIndex;

    // Esto es para un test de que el pooler funciona bien cambiando de escena. Borraremos mas adelante
    public void ToggleScene()
    {
        currSceneIndex = 1 + (currSceneIndex % 2);
        SceneManager.LoadScene(currSceneIndex);
    }

    protected override void OnEndImpl() {}
    protected override void OnBeginImpl() {

        DontDestroyOnLoad(this);
        List<int> scenes = new List<int>();
        
        Scene currScene = SceneManager.GetActiveScene();
        #if UNITY_EDITOR
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene s = SceneManager.GetSceneAt(i);
            if (currScene != s && s.buildIndex != -1)
                scenes.Add(SceneManager.GetSceneAt(i).buildIndex);
                
            scenes.ForEach(s => DeactivateAllObjects(s));  
        }
        #endif
      
        WaitForScriptableObjects(scenes);
    }

    private async void WaitForScriptableObjects(List<int> scenes)
    {
        while (true) {
            if (ScriptableObjectsDependencies.All(so => so.IsLoaded)) break;
            await Task.Delay(100);
        }

        LoadInitialScene(scenes);
    }
    
    private void LoadInitialScene(List<int> availableScenes) 
    {
        currSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // En el caso de estar en la escena Preload, si o si, hay que cargar la "siguiente". 
        if (currSceneIndex == PreloadScene) {
            if (availableScenes.Count > 0) {
                // Si hay escenas disponibles que esten abiertas en el editor, elegimos arbitrariamente
                currSceneIndex = availableScenes[0];
            } else {
                // Si no hay escenas disponibles abiertas en el editor, pasmos directamente a la escena Default, definida como constante
                currSceneIndex = DefaultScene;
            }
            SceneManager.LoadScene(currSceneIndex);
            ActivateAllObjects(currSceneIndex);
        } 
    }

    // Idealmente esto nos permitiria que no corra ningun game object ANTES de que la inicializacion de los scriptable objects termine
    // peroo eso parece ser imposible asi que por el momento dejo estos aca hasta que veamos si nos sirve la logica eventualmente
    private void DeactivateAllObjects(int sceneBuildIndex) 
    {
        Scene s = SceneManager.GetSceneByBuildIndex(sceneBuildIndex);
        s.GetRootGameObjects().ToList().ForEach(go => go.SetActive(false));
        SceneManager.UnloadSceneAsync(s);
    } 
    
    private void ActivateAllObjects(int sceneBuildIndex) 
    {
        Scene s = SceneManager.GetSceneByBuildIndex(sceneBuildIndex);
        s.GetRootGameObjects().ToList().ForEach(go => go.SetActive(true));
    } 
}
