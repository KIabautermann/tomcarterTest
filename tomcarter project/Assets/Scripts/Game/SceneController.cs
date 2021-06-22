using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

// No sirve para nada... Me copa la idea de tener esto al menos para el editor y asegurarnos de que los 
// Scriptable objects globales se inicialicen antes que los Game Objects de las escenas, but alas, 
// es fucking imposible por lo que veo. Si queremos usar el editor y darle play SIN tener que unloadear una
// escena, estamos en la b
public class SceneController : MonoBehaviour
{
    [SerializeField]
    public List<PersistedScriptableObject> ScriptableObjectsDependencies;
    private void OnEnable() {
     
        List<Scene> scenes = new List<Scene>();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (gameObject.scene != SceneManager.GetSceneByBuildIndex(i))
                scenes.Add(SceneManager.GetSceneByBuildIndex(i));
        }
        scenes.ForEach(s => DeactivateAllObjects(s));        
        
        StartCoroutine(WaitForScriptableObjects(scenes));
        
        DontDestroyOnLoad(this);
    }

    private IEnumerator WaitForScriptableObjects(List<Scene> scenes)
    {
        while (true) {
            if (ScriptableObjectsDependencies.All(so => so.IsLoaded)) break;
            yield return new WaitForEndOfFrame();
        }
        scenes.ForEach(s => ActivateAllObjects(s));
    }
    
    private void DeactivateAllObjects(Scene s) 
    {
        s.GetRootGameObjects().ToList().ForEach(go => go.SetActive(false));
        SceneManager.UnloadSceneAsync(s);
    } 
    
    private void ActivateAllObjects(Scene s) 
    {
        s.GetRootGameObjects().ToList().ForEach(go => go.SetActive(true));
        SceneManager.LoadScene(s.name);
    } 
}
