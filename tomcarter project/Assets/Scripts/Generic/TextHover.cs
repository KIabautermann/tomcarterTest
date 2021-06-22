using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextHover : PoolableObject
{
    protected override void Start()
    {
        base.Start();
    }
    public override void ResetSceneReferences() {}
    
    [SerializeField]
    public TextMeshProUGUI textLabel;

    private void Update() {
        // Vector3 textPos = Camera.main.WorldToScreenPoint(this.transform.position);
        // textLabel.transform.position = textPos;
    }
}
