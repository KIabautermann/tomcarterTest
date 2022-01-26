using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CanvasReferencer", menuName = "Canvas Extensions")]
public class CanvasReference : PersistedScriptableObject
{
    public SceneControllerObject SceneController;
    public ObjectPooler TextHoverPooler;
    public GameObject CanvasPrefab;
    private GameObject _canvas;
    private Dictionary<CanvasElement, TextMeshProUGUI> _textMeshes;
    private Dictionary<CanvasElement, Image> _images;
    private Dictionary<GameObject, ComponentCache<MonoBehaviour>> _poolableMeshesComponents;
    protected override void OnBeginImpl() {
        _canvas = GameObject.FindGameObjectWithTag("Canvas");
        if (_canvas == null)
        {
            _canvas = Instantiate(CanvasPrefab, Vector3.zero, Quaternion.identity);
        }  
        _textMeshes = _canvas.GetComponentsInChildren<TextMeshProUGUI>()
            .Where(m => m.gameObject.activeSelf)
            .Select(m => new Tuple<CanvasElement?, TextMeshProUGUI>(GetCanvasElement(m.gameObject.name), m))
            .Where(t => t.Item1 != null)
            .Select(t => new Tuple<CanvasElement, TextMeshProUGUI>((CanvasElement)t.Item1, t.Item2))
            .ToDictionary(t => t.Item1, t => t.Item2); 

        _poolableMeshesComponents = new Dictionary<GameObject, ComponentCache<MonoBehaviour>>();

        _images = _canvas.GetComponentsInChildren<Image>()
            .Where(m => m.gameObject.activeSelf)
            .Select(m => new Tuple<CanvasElement?, Image>(GetCanvasElement(m.gameObject.name), m))
            .Where(t => t.Item1 != null)
            .Select(t => new Tuple<CanvasElement, Image>((CanvasElement)t.Item1, t.Item2))
            .ToDictionary(t => t.Item1, t => t.Item2);

        DontDestroyOnLoad(_canvas);
    }
    protected override void OnEndImpl() {}

    // Esto es nefasto pero habria que mejorarlo. En esencia estamos medio a manopla eligiendo de donde obtener la instancia del TextMeshPro
    // segun que elemento pedimos. Como algunos son pooleables, como el HoverText, entonces tomamos un camino alternativo, comparado a alguno
    // que tendria una instancia unica ya definida en el prefab
    public TextMeshProUGUI GetTextMeshForGameObject(CanvasElement element)
    {
        switch (element) {
            case CanvasElement.TextHover:
                // Ir pensando como devolver estos con un cambio de escena
                var cache = TextHoverPooler.GetItem(Vector3.zero, Quaternion.identity);
                cache.GetInstance(typeof(TextMeshProUGUI), out MonoBehaviour tmp);
                tmp.gameObject.transform.SetParent(_canvas.transform); 
                _poolableMeshesComponents[tmp.gameObject] = cache;
                return tmp as TextMeshProUGUI;
        }
        return _textMeshes[element];
    } 

    public Image GetImageForGameObject(CanvasElement element)
    {
        return _images[element];
    }

    private CanvasElement? GetCanvasElement(string gameObjectName) 
    {
        CanvasElement? elem = null;
        switch (gameObjectName) {
            case "TopBanner":
                elem = CanvasElement.TopBanner;
                break;
            case "DialogueText":
                elem = CanvasElement.PupupDialogueBox;
                break;
            case "HoverText":
                elem = CanvasElement.TextHover;
                break;
            case "ToggleSceneText":
                elem = CanvasElement.SceneButton;
                break;
            case "Health Meter":
                elem = CanvasElement.HealthMeter;
                break;
        }
        return elem;
    }

    public void ReturnHoverTextMesh(TextMeshProUGUI textMesh)
    {
        _poolableMeshesComponents[textMesh.gameObject].GetInstance(typeof(PoolableObject), out MonoBehaviour tmp);
        _poolableMeshesComponents.Remove(textMesh.gameObject);
        PoolableObject poolable = tmp as PoolableObject;
        poolable.Dispose();
    }
}
