using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextHoverUI : PoolableObject
{
    private TextMeshProUGUI textMesh;
    private GameObject poolerParent;
    public override void ResetSceneReferences() {
        this.gameObject.transform.SetParent(poolerParent.transform);
        textMesh.text = "";   
        gameObject.SetActive(false);
    }
    protected override void Start() {
        textMesh = GetComponent<TextMeshProUGUI>();
        poolerParent = this.gameObject.transform.parent.gameObject;
        base.Start();
    }
}
