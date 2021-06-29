using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

[CreateAssetMenu(fileName = "NewObjectPooler", menuName = "Pooler")]
public class ObjectPooler : PersistedScriptableObject
{    
    public string poolName;
    private GameObject parent;
    public GameObject prefabObject;
    public int instanceAmount;
    private Queue<GameObject> objectPool;
    private Dictionary<int, ISet<GameObject>> borrowedElements;
    private Dictionary<GameObject, PoolableObject> poolableObjectComponents;
    private Dictionary<GameObject, ComponentCache<MonoBehaviour>> componentCache;
    private int currentSceneIndex;

    // Devuelve una Cache de Componentes para evitar tener que pedir Scripts especificos cada vez que se Borrowea un GameObject 
    // pooleable. Lo mas logico es que si alguien pide un objeto, es para referenciar a ciertos componentes suyos
    // Si mas adelante, por ejemplo para proyectiles, queremos instanciar 200, 200 GetComponent<ProjectileScript>() nos
    // saldria bastante caro
    public ComponentCache<MonoBehaviour> GetItem(Vector3 position, Quaternion quaternion)
    {
        GameObject gameObject = objectPool.Dequeue();
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        ISet<GameObject> set = borrowedElements.ContainsKey(sceneIndex) 
            ? borrowedElements[sceneIndex]
            : new HashSet<GameObject>();

        // Si el game object esta activo, quiere decir que esta en uso y prestado
        // Por lo tanto el pooler lo intentara recolectar, resetteandolo en el proceso
        if (gameObject.activeSelf) DisposePoolable(poolableObjectComponents[gameObject]);
        
        // Indicar que este game object esta en uso en la escena actualmente activa
        set.Add(gameObject);
        borrowedElements[sceneIndex] = set;

        // Configuralo de manera inicial
        gameObject.SetActive(true);
        gameObject.transform.position = position;
        gameObject.transform.rotation = quaternion;

        // Lo volvemos a encolar ya que esto es un pooler circular
        objectPool.Enqueue(gameObject);

        return componentCache[gameObject];
    }

    protected override void OnBeginImpl() {
        objectPool = new Queue<GameObject>();
        borrowedElements = new Dictionary<int, ISet<GameObject>>();
        poolableObjectComponents = new Dictionary<GameObject, PoolableObject>();
        componentCache = new Dictionary<GameObject, ComponentCache<MonoBehaviour>>();

        parent = new GameObject();
        parent.name = poolName;
        DontDestroyOnLoad(parent);

        for (int i = 0; i < instanceAmount; i++) {
            GameObject instance = Instantiate(prefabObject, Vector3.zero, Quaternion.identity, parent.transform);
            objectPool.Enqueue(instance);
            PoolableObject poolableObject = instance.GetComponent<PoolableObject>();
            poolableObject.DisposalRequested += DisposeFreeObject;
            poolableObjectComponents[instance] = poolableObject;
            componentCache[instance] = new ComponentCache<MonoBehaviour>(instance.GetComponentsInChildren<MonoBehaviour>(), new List<MonoBehaviour>());
        }
        
        SceneManager.activeSceneChanged += FreePooledObjects;
        DontDestroyOnLoad(this);
    }

    // Hanlder para el Pooler cuando un PoolableObject avisa que el borrower lo libero
    private void DisposeFreeObject(object sender, EventArgs eventArgs) 
    {
        PoolableObject poolable = (PoolableObject) sender;
        GameObject gameObject = poolable.gameObject;
        DisposePoolable(poolable);
        borrowedElements[SceneManager.GetActiveScene().buildIndex].Remove(gameObject);
    }

    // Handler para cuando hay un cambio de escenas y hace falta recolectar todos los objetos prestados en la escena
    private void FreePooledObjects(Scene current, Scene next)
    {   
        int previousScene = currentSceneIndex;
        currentSceneIndex = next.buildIndex;
        if (previousScene == -1 || !borrowedElements.ContainsKey(previousScene)) return; 

        foreach (GameObject gameObject in borrowedElements[previousScene]) {
            DisposePoolable(gameObject.GetComponent<PoolableObject>());
            gameObject.SetActive(false);
        }
        borrowedElements.Remove(previousScene);
    }

    // Llama al OnDispose del objeto y desactiva su gameObject
    private void DisposePoolable(PoolableObject poolable)
    {
        poolable.OnDispose();
        poolable.gameObject.SetActive(false);
    }
    protected override void OnEndImpl() 
    { 
        // No se si hace falta realmente destruir estos objetos ya que este OnEnd se llama al frenar la ejecucion del juego
        // Pero por ahi es mejor tenerlo para el modo play del editor cosa de que no sobreviva ningun objeto
        // BURN THEM ALL!
        if (objectPool != null ) objectPool.ToList().ForEach(go => DestroyImmediate(go));
        DestroyImmediate(parent);
        objectPool = new Queue<GameObject>();
    }

}
