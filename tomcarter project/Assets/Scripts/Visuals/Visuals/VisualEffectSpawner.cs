﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

[CreateAssetMenu(fileName = "EffectSpawner", menuName = "VisualEffectSpawner")]
public class VisualEffectSpawner : ObjectPooler
{    
    public EffectAnimationRepository EffectRepository;

    public VisualEffectPoolable InstanceEffect(GameObject parent, Vector3 position, Quaternion quaternion, AnimationClip animationClip)
    {
        ComponentCache<MonoBehaviour> afterImageComponents = GetItem(position, quaternion);
        afterImageComponents.GetInstance(typeof(VisualEffectPoolable), out MonoBehaviour tmp);
        VisualEffectPoolable pais = tmp as VisualEffectPoolable;
        pais.gameObject.transform.SetParent(parent.transform, true);
        pais.LogicStart(animationClip);
        
        return pais;
    }

    public VisualEffectPoolable InstanceEffect(GameObject parent, Vector3 position, Quaternion quaternion, int stateIndex, int animationIndex)
    {
        ComponentCache<MonoBehaviour> afterImageComponents = GetItem(position, quaternion);
        afterImageComponents.GetInstance(typeof(VisualEffectPoolable), out MonoBehaviour tmp);
        VisualEffectPoolable pais = tmp as VisualEffectPoolable;
        pais.gameObject.transform.SetParent(parent.transform, true);
        pais.LogicStart(stateIndex, animationIndex);
        
        return pais;
    }

}