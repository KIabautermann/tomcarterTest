using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

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
        SceneManager.LoadSceneAsync(currSceneIndex);
    }

    protected override void OnEndImpl() {}
    protected override void OnBeginImpl() {
     
        DontDestroyOnLoad(this);
        List<Scene> scenes = new List<Scene>();
        Scene currScene = SceneManager.GetActiveScene();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene s = SceneManager.GetSceneByBuildIndex(i);
            if (currScene != s && s.buildIndex != -1)
                scenes.Add(SceneManager.GetSceneByBuildIndex(i));
        }

        scenes.ForEach(s => DeactivateAllObjects(s));        
        
        WaitForScriptableObjects(scenes);
    }

    private void WaitForScriptableObjects(List<Scene> scenes)
    {
        while (true) {
            if (ScriptableObjectsDependencies.All(so => so.IsLoaded)) break;
        }
        scenes.ForEach(s => ActivateAllObjects(s));

        LoadInitialScene(scenes);
    }
    
    private void LoadInitialScene(List<Scene> availableScenes) 
    {
        currSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (SceneManager.GetActiveScene().buildIndex == PreloadScene && availableScenes.Count > 0) {
            if (availableScenes.Count > 0) {
                currSceneIndex = availableScenes[0].buildIndex;
            } else {
                currSceneIndex = DefaultScene;
            }
            SceneManager.LoadSceneAsync(currSceneIndex);
        } 
    }

    // Idealmente esto nos permitiria que no corra ningun game object ANTES de que la inicializacion de los scriptable objects termine
    // peroo eso parece ser imposible asi que por el momento dejo estos aca hasta que veamos si nos sirve la logica eventualmente
    private void DeactivateAllObjects(Scene s) 
    {
        s.GetRootGameObjects().ToList().ForEach(go => go.SetActive(false));
        SceneManager.UnloadSceneAsync(s);
    } 
    
    private void ActivateAllObjects(Scene s) 
    {
        s.GetRootGameObjects().ToList().ForEach(go => go.SetActive(true));
    } 
}
