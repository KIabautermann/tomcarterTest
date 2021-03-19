using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public Dictionary<string, Queue<GameObject>> poolDictionaty;
    void Start()
    {
        poolDictionaty = new Dictionary<string, Queue<GameObject>>();
    }

    
}
