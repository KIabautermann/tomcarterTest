using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseDashState : PlayerDashState
{
    private ObjectPooler afterImagePooler;
    
    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        animationTrigger = stats.dashID;
        afterImagePooler = target.afterImagePooler;
    }
    protected override void DoChecks()
    {
        base.DoChecks();
    }
    protected override void DoLogicUpdate()
    {
        if (controller.OnHedge() && _hedgeUnlocked && inputs.FixedAxis.y < 0) {
            direction = Vector2.down;
        }

        base.DoLogicUpdate();
    }
    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
    }
    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        Debug.Log(animationIndex);
        currentSpeed = stats.dashSpeed;
        direction = new Vector2(controller.facingDirection,0);
    }
    protected override void DoTransitionOut()
    {
        StopAllCoroutines();
        base.DoTransitionOut();
    }
    protected override void TransitionChecks()
    {
        base.TransitionChecks();
        if(counter > + stats.dashLenght && !stateDone)
        {
            stateDone = true;
            controller.SetDrag(0);
            controller.SetGravity(true);
        }
    }
    protected override IEnumerator InstanceAfterImage()
    { 
        while (true) {
            
            yield return new WaitForSeconds(0.035f);

            ComponentCache<MonoBehaviour> afterImageComponents = afterImagePooler.GetItem(Vector3.zero, Quaternion.identity);
            afterImageComponents.GetInstance(typeof(PlayerAfterImageSprite), out MonoBehaviour tmp);
            PlayerAfterImageSprite pais = tmp as PlayerAfterImageSprite;

            pais.gameObject.transform.SetParent(afterImageParent.transform, true);
            
            if (controller.facingDirection != 1) pais.gameObject.transform.Rotate(0.0f, 180.0f, 0.0f);

            pais.LogicStart(this.gameObject.transform.position, stateIndex, animationIndex, Mathf.RoundToInt(counter - stats.dashStartUp));
        }
    }

    protected override void ChangeAnimationIndex() 
    {
    }

    private void OnDestroy() {
        Destroy(afterImageParent);
    }
}
