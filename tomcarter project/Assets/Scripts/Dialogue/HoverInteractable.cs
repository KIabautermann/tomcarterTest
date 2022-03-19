using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HoverInteractable : PoolableObject
{
    private TextMeshProUGUI _hoverText;
    private RectTransform rt;
    private Vector3 hoverPosition;
    [SerializeField]
    public CanvasReference CanvasReference;
    public override void ResetSceneReferences()
    {
        CanvasReference.ReturnHoverTextMesh(_hoverText);
        _hoverText = null;
    }

    protected override void Start() 
    {    
        base.Start();
    }

    public void LogicStart(string hoverText, Vector3 hoverPosition)
    {
        _hoverText = CanvasReference.GetTextMeshForGameObject(CanvasElement.TextHover);
        GameObject gameObject = _hoverText.gameObject;
        rt = gameObject.GetComponent<RectTransform>();
        gameObject.SetActive(true);

        _hoverText.text = hoverText;
        this.hoverPosition = hoverPosition;
        Vector3 textPos = Camera.main.WorldToScreenPoint(hoverPosition);
        rt.transform.position = textPos;
    }

    private void Update() {
        Vector3 textPos = Camera.main.WorldToScreenPoint(hoverPosition);
        rt.transform.position = textPos;    
    }


}
