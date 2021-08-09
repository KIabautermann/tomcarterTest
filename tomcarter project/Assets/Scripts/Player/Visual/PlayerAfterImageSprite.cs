using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImageSprite : PoolableObject
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

    public void LogicStart(Vector3 position, int state, int animationIndex, int startingTime)
    {
        this.gameObject.transform.position = position;
        this.gameObject.transform.localPosition = position;
        this.stateID = state;
        StartCoroutine(FadeSprite(state, animationIndex, startingTime));
    }

    private IEnumerator FadeSprite(int state, int animationIndex, int startingTime) 
    {
        yield return new WaitForEndOfFrame();

        animationController.PlayAnimation(state, animationIndex);
        
        yield return new WaitForSeconds(0.2f);
        Dispose();
    }
    public void SetAnimationIndex(int newIndex) {
        animationController.PlayAnimation(stateID, newIndex);
    }
}
