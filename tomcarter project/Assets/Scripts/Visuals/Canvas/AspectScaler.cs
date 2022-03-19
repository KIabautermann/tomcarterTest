using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectScaler : MonoBehaviour
{
    CustomCanvasScaler scaler;
    [Range(1,30)]
    public int screenMult;
    void Start()
    {
        scaler = GetComponent<CustomCanvasScaler>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        scaler.referenceResolution.Set(screenMult * 16, screenMult * 9);
    }
}
