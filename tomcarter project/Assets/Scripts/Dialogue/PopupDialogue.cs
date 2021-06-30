using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class PopupDialogue : PoolableObject
{
    private TextMeshProUGUI _dialogueText;
    [SerializeField]
    private CanvasReference CanvasReference;
    [SerializeField]
    public Vector2 padding;
    [SerializeField]
    public Vector2 maxSize = new Vector2(1000, float.PositiveInfinity);
    [SerializeField]
    public Vector2 minSize;
    [SerializeField]
    public float talkingSpeed = 0.05f;
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

    private bool _isLocked = false;
    private Coroutine writtingCorouting;

     protected virtual float MinX { get {
            if ((controlAxes & Mode.Horizontal) != 0) return minSize.x / ScreenScale.x;
            return rt.rect.width - padding.x / ScreenScale.x;
        } }
    protected virtual float MinY { get {
            if ((controlAxes & Mode.Vertical) != 0) return minSize.y  / ScreenScale.y;
            return rt.rect.height - padding.y / ScreenScale.y;
        } }
    protected virtual float MaxX { get {
            if ((controlAxes & Mode.Horizontal) != 0) return maxSize.x  * ScreenScale.x;
            return rt.rect.width - padding.x  / ScreenScale.x;
        } }
    protected virtual float MaxY { get {
            if ((controlAxes & Mode.Vertical) != 0) return maxSize.y  * ScreenScale.y;
            return rt.rect.height - padding.y  / ScreenScale.y;
        } }

    private Vector2 ScreenScale
    {
        get
        {
            if (canvasScaler == null)
            {
                canvasScaler = GetComponentInParent<CanvasScaler>();
            }

            if (canvasScaler)
            {
                return new Vector2(canvasScaler.referenceResolution.x / Screen.width, canvasScaler.referenceResolution.y / Screen.height);
            }
            else
            {
                return Vector2.one;
            }
        }
    }

    private RectTransform rt;
    private RectTransform parentRt;
    private CanvasScaler canvasScaler;
    
    protected override void Start() 
    {    
        _dialogueText = CanvasReference.GetTextMeshForGameObject(CanvasElement.PupupDialogueBox);
        GameObject gameObject = _dialogueText.gameObject;
        rt = gameObject.GetComponent<RectTransform>();
        parentRt = gameObject.transform.parent.GetComponent<RectTransform>();
        parentRt.gameObject.SetActive(false);
        canvasScaler = gameObject.GetComponentInParent<CanvasScaler>();
        base.Start();
    }
    
    public override void ResetSceneReferences()
    {
        _dialogueText.text = "";   
        _dialogueText = null;
        parentRt.gameObject.SetActive(false);
    }

    public void LogicStart(Vector3 textPos)
    {
        _dialogueText = CanvasReference.GetTextMeshForGameObject(CanvasElement.PupupDialogueBox);
        parentRt.transform.position = textPos;
        rt.transform.position = textPos;
    }

    public void LogicEnd()
    {
        parentRt.gameObject.SetActive(false);
        _dialogueText.text = "";
    }
    public void Display(string text, Vector3 textPos)
    {
        if (_isLocked) return;

        parentRt.transform.position = textPos;
        rt.transform.position = textPos;
        _dialogueText.text = text;
        Resize();
        parentRt.gameObject.SetActive(true);
        writtingCorouting = StartCoroutine(WriteTextToPopup(text));
        _isLocked = true;
    }

    public void EndLine()
    {
        StopCoroutine(writtingCorouting);
        _dialogueText.text = lastText;
        _isLocked = false;
    }

    public bool IsLocked() => _isLocked;
    private IEnumerator WriteTextToPopup(string text) 
    {
        _dialogueText.text = "";
        foreach (char c in text) {
            _dialogueText.text += c;
            yield return new WaitForSeconds(talkingSpeed);
        }
        
        _isLocked = false;
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
