using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextHover : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI textLabel;

    private void Update() {
        Vector3 textPos = Camera.main.WorldToScreenPoint(this.transform.position);
        textLabel.transform.position = textPos;
    }
}
