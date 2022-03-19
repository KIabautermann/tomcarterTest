using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Materials Repository", menuName = "Materials Repository")]
public class MaterialsRepository : ScriptableObject
{
    public Material defaultMaterial;
    public Material hitMaterial;
}
