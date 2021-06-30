using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Threading.Tasks;

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
        List<int> scenes = new List<int>();
        
        Scene currScene = SceneManager.GetActiveScene();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene s = SceneManager.GetSceneAt(i);
            if (currScene != s && s.buildIndex != -1)
                scenes.Add(SceneManager.GetSceneAt(i).buildIndex);
        }

        scenes.ForEach(s => DeactivateAllObjects(s));        
        
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

        if (SceneManager.GetActiveScene().buildIndex == PreloadScene) {
            if (availableScenes.Count > 0) {
                currSceneIndex = availableScenes[0];
            } else {
                currSceneIndex = DefaultScene;
            }
            SceneManager.LoadSceneAsync(currSceneIndex);
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
