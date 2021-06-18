using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "NewObjectPooler", menuName = "Pooler")]
[InitializeOnLoad]
public class ObjectPooler : ScriptableObject
{    
    public string poolName;
    private GameObject parent;
    public GameObject prefabObject;
    public int instanceAmount;
    private Queue<GameObject> objectPool;
    private Dictionary<int, ISet<GameObject>> borrowedElements;

    public GameObject GetItem(Vector3 position, Quaternion quaternion)
    {
        GameObject gameObject = objectPool.Dequeue();
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        ISet<GameObject> set = borrowedElements.ContainsKey(sceneIndex) 
            ? borrowedElements[sceneIndex]
            : new HashSet<GameObject>();

        set.Add(gameObject);
        borrowedElements[sceneIndex] = set;

        gameObject.SetActive(true);
        gameObject.transform.position = position;
        gameObject.transform.rotation = quaternion;

        objectPool.Enqueue(gameObject);

        return gameObject;
    }

    private void OnBegin() {
        objectPool = new Queue<GameObject>();
        borrowedElements = new Dictionary<int, ISet<GameObject>>();

        parent = new GameObject();
        parent.name = poolName;

        for (int i = 0; i < instanceAmount; i++) {
            GameObject instance = Instantiate(prefabObject, Vector3.zero, Quaternion.identity, parent.transform);
            instance.SetActive(false);
            objectPool.Enqueue(instance);
            instance.GetComponent<PoolableObject>().DisposalRequested += DisposeFreeObject;
        }
        
        SceneManager.activeSceneChanged += FreePooledObjects;
        DontDestroyOnLoad(this);
    }

    private void DisposeFreeObject(object sender, EventArgs eventArgs) 
    {
        GameObject poolable = ((PoolableObject) sender).gameObject;
        borrowedElements[SceneManager.GetActiveScene().buildIndex].Remove(poolable);
        poolable.SetActive(false);
    }

    private void FreePooledObjects(Scene current, Scene next)
    {
        foreach (GameObject gameObject in borrowedElements[current.buildIndex]) {

            PoolableObject poolable = gameObject.GetComponent<PoolableObject>();
            poolable.ResetSceneReferences();
            //objectPool.Enqueue(gameObject);
        }
        borrowedElements.Remove(current.buildIndex);
    }
    private void OnEnd() 
    { 
        // No se si hace falta realmente destruir estos objetos ya que este OnEnd se llama al frenar la ejecucion del juego
        if (objectPool != null ) objectPool.ToList().ForEach(go => DestroyImmediate(go));
        DestroyImmediate(parent);
        objectPool = new Queue<GameObject>();
    }

    #if UNITY_EDITOR
        protected void OnEnable()
        {
            EditorApplication.playModeStateChanged += OnPlayStateChange;
        }
 
        protected void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayStateChange;
        }
 
        void OnPlayStateChange(PlayModeStateChange state)
        {
            if(state == PlayModeStateChange.EnteredPlayMode)
            {
                OnBegin();
            }
            else if(state == PlayModeStateChange.ExitingPlayMode)
            {
                OnEnd();
            }
        }
    #else
        protected void OnEnable()
        {
            OnBegin();
        }
 
        protected void OnDisable()
        {
            OnEnd();
        }
    #endif
}
