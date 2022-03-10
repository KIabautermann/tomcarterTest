using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
        
public class Preloader : MonoBehaviour
{
    [SerializeField]
    public CanvasReference CanvasReference;
    [SerializeField]
    public SceneControllerObject SceneControllerObject;
    [SerializeField]
    public VisualEffectSpawner VisualEffectSpawner;
    [SerializeField]
    public List<ObjectPooler> Poolers;
    // [SerializeField]
    // public List<SceneRefer> Scenes;

}
