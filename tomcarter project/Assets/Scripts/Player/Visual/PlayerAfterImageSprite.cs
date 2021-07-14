using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImageSprite : PoolableObject
{
    private GameObject poolerParent;
    public override void ResetSceneReferences()
    {
        this.gameObject.transform.position = Vector3.zero;
        animationController.StopAllCoroutines();
        this.gameObject.transform.SetParent(poolerParent.transform);
    }

    private SpriteRenderer spriteRenderer;
    private AnimationController animationController;

    // Start is called before the first frame update
    protected override void Start()
    {
        animationController = GetComponent<AnimationController>();
        poolerParent = this.gameObject.transform.parent.gameObject;
        //Debug.Log("starting after image");
        base.Start();
        // spriteRenderer = GetComponent<SpriteRenderer>();
        // var renderer = GetComponent<Renderer>();
        // Debug.Log(renderer);
        // spriteRenderer.color = Color.clear;
    }

    public void LogicStart(Vector3 position, int state, int animationIndex, int startingTime)
    {
        Debug.Log("logic start");
        Debug.Log(gameObject.transform.parent.name);
        Debug.Log(gameObject.transform.parent.position);
        Debug.Log(gameObject.transform.position);
        this.gameObject.transform.position = position;
        StartCoroutine(FadeSprite(state, animationIndex, startingTime));
    }

    private IEnumerator FadeSprite(int state, int animationIndex, int startingTime) 
    {
        yield return new WaitForEndOfFrame();

        animationController.PlayAnimation(state, animationIndex);
        float transparency = 255;
        
        //float transparency = spriteRenderer.color.a;
        while (transparency > 0) {
            yield return new WaitForSeconds(0.9f);
            //Color currColor = spriteRenderer.color;
            transparency -= 20f;
            //spriteRenderer.color = new Color(currColor.r, currColor.g, currColor.b, Mathf.Max(transparency, 0));
        }

        Dispose();
    }
}
