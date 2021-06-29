using System.Collections;
using System.Collections.Generic;
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
    protected override void OnBeginImpl() {
        _canvas = GameObject.FindGameObjectWithTag("Canvas");
        if (_canvas == null)
        {
            _canvas = Instantiate(CanvasPrefab, Vector3.zero, Quaternion.identity);
        }  
        _textMeshes = _canvas.GetComponentsInChildren<TextMeshProUGUI>()
            .Where(m => m.gameObject.activeSelf)
            .ToDictionary(m => GetCanvasElement(m.gameObject.name), m => m); 

        DontDestroyOnLoad(_canvas);
    }
    protected override void OnEndImpl() {}

    // Esto es nefasto pero habria que mejorarlo. En esencia estamos medio a manopla eligiendo de donde obtener la instancia del TextMeshPro
    // segun que elemento pedimos. Como algunos son pooleables, como el DialogueBox, entonces tomamos un camino alternativo, comparado a alguno
    // que tendria una instancia unica
    public TextMeshProUGUI GetTextMeshForGameObject(CanvasElement element)
    {
        switch (element) {
            case CanvasElement.TextHover:
                // Ir pensando como devolver estos con un cambio de escena
                TextHoverPooler.GetItem(Vector3.zero, Quaternion.identity).GetInstance(typeof(TextMeshProUGUI), out MonoBehaviour tmp);
                return tmp as TextMeshProUGUI;
        }
        return _textMeshes[element];
    } 

    private CanvasElement GetCanvasElement(string gameObjectName) 
    {
        CanvasElement elem;
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
            default:
                throw new System.Exception($"Couldn't map canvas game object {gameObjectName} to an element Enum");
        }
        return elem;
    }
}
