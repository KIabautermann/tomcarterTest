using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HedgeColorData", menuName = "Data/Colors/HedgeColors")]
public class HedgeColors : ScriptableObject
{
    public Color enabledA;
    public Color disabledA;
    public Color enabledB;
    public Color disabledB;
    public Color enabledC;
    public Color disabledC;
}
