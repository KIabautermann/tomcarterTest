using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;

public class SoundManager : MonoBehaviour
{
    [Range(0, 1)]
    public int bossValue;
    [Range(0, 1)]
    public float actionValue;
    FMOD.Studio.EventInstance Instance;

    private void Start()
    {
        Instance = FMODUnity.RuntimeManager.CreateInstance("event:/Tutorial");
        Instance.start();
    }

    private void Update()
    {
        Instance.setParameterByName("Boss", bossValue);
        Instance.setParameterByName("Action", actionValue);
    }
}
