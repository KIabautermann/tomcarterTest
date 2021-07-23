using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImageSprite : PoolableObject
{
    private GameObject poolerParent;
    public override void ResetSceneReferences()
    {
        this.gameObject.transform.position = Vector3.zero;
        StopAllCoroutines();
        this.gameObject.transform.SetParent(poolerParent.transform);
    }

    private SpriteRenderer spriteRenderer;
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
        StartCoroutine(FadeSprite(state, animationIndex, startingTime));
    }

    private IEnumerator FadeSprite(int state, int animationIndex, int startingTime) 
    {
        yield return new WaitForEndOfFrame();

        animationController.PlayAnimationAtStart(state, animationIndex, 0f);
        
        float transparency = 255;
        
        //float transparency = spriteRenderer.color.a;
        while (transparency > 0) {
            yield return new WaitForSeconds(0.008f);
            //Color currColor = spriteRenderer.color;
            transparency -= 20f;
            //spriteRenderer.color = new Color(currColor.r, currColor.g, currColor.b, Mathf.Max(transparency, 0));
        }

        Dispose();
    }
}
