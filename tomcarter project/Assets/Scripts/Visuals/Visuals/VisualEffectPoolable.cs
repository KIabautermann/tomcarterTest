using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VisualEffectPoolable : PoolableObject
{
    private int stateID;
    private GameObject poolerParent;
    public override void ResetSceneReferences()
    {
        this.gameObject.transform.position = Vector3.zero;
        StopAllCoroutines();
        this.gameObject.transform.SetParent(poolerParent.transform);
    }
    private AnimationController animationController;

    protected override void Start()
    {
        animationController = GetComponent<AnimationController>();
        poolerParent = this.gameObject.transform.parent.gameObject;
        base.Start();
    }

    // Esto nos trae un problemon de: que pasa si el clip tiene un Animation Event? Hay que implementar esa funcion para que pueda ser encontrada
    // Habria que cambiarla para que pase un AnimationClip en vez de un nuevo indice?
    public void LogicStart(AnimationClip animationClip)
    {
        PlayAndReturn(animationClip);
    }

    public void LogicStart(int state, int animationIndex)
    {
        this.stateID = state;
        PlayAndReturn(state, animationIndex);
    }

    private void PlayAndReturn(int state, int animationIndex) 
    {        
        StartCoroutine(PlayAndReturn(() => animationController.PlayAnimation(state, animationIndex)));
    }

    private void PlayAndReturn(AnimationClip animationClip) 
    {
        StartCoroutine(PlayAndReturn(() => animationController.PlayAnimation(animationClip)));
    }

    private IEnumerator PlayAndReturn(Action play) 
    {
         // Espera un frame para que se termine de traer este game object a la escena
        yield return new WaitForEndOfFrame();
        
        play();
        
        // Espera para hacerlo desaparecer
        yield return new WaitForSeconds(0.5f);

        // Devolverlo
        Dispose();
    }
    
    public void SetAnimationIndex(int newIndex) {
        animationController.PlayAnimation(stateID, newIndex);
    }
}
