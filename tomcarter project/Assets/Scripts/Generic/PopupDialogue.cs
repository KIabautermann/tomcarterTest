using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupDialogue : PoolableObject
{
    [SerializeField]
    private TextMeshProUGUI _dialogueText;
    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    public Vector2 padding;
    [SerializeField]
    public Vector2 maxSize = new Vector2(1000, float.PositiveInfinity);
    [SerializeField]
    public Vector2 minSize;
    public enum Mode
    {
        None        = 0,
        Horizontal  = 0x1,
        Vertical    = 0x2,
        Both        = Horizontal | Vertical
    }
    
    private Mode controlAxes = Mode.Both;
    private string lastText = null;
    private Vector2 lastSize;

     protected virtual float MinX { get {
            if ((controlAxes & Mode.Horizontal) != 0) return minSize.x;
            return rt.rect.width - padding.x;
        } }
    protected virtual float MinY { get {
            if ((controlAxes & Mode.Vertical) != 0) return minSize.y;
            return rt.rect.height - padding.y;
        } }
    protected virtual float MaxX { get {
            if ((controlAxes & Mode.Horizontal) != 0) return maxSize.x;
            return rt.rect.width - padding.x;
        } }
    protected virtual float MaxY { get {
            if ((controlAxes & Mode.Vertical) != 0) return maxSize.y;
            return rt.rect.height - padding.y;
        } }

    private RectTransform rt;
    private RectTransform parentRt;
    
    private void Start() 
    {    
        rt = GetComponent<RectTransform>();
        parentRt = gameObject.GetComponentInParent<RectTransform>();
    }
    
    public override void ResetSceneReferences()
    {
        _dialogueText.text = "";   
        _dialogueText = null;
    }

    public void LogicStart(TextMeshProUGUI dialogueText)
    {
        _dialogueText = dialogueText;   
    }

    public void Display(string text)
    {
        _dialogueText.text = text;
        Resize();
    }

    private void Resize() 
    {
        if (_dialogueText.text != lastText || lastSize != rt.rect.size )
        {
            lastText = _dialogueText.text;
            Vector2 preferredSize = _dialogueText.GetPreferredValues(MaxX, MaxY);
            preferredSize.x = Mathf.Clamp(preferredSize.x, MinX, MaxX);
            preferredSize.y = Mathf.Clamp(preferredSize.y, MinY, MaxY);
            preferredSize += padding;
 
            if ((controlAxes & Mode.Horizontal) != 0)
            {
                rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, preferredSize.x);
                parentRt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, preferredSize.x);
            }
            if ((controlAxes & Mode.Vertical) != 0)
            {
                rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, preferredSize.y);
                parentRt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, preferredSize.y);
            }
            lastSize = rt.rect.size;
        }
    }

}
