using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseDashState : PlayerDashState
{
    private GameObject afterImageParent;
    [SerializeField]
    private VisualEffectSpawner visualEffectSpawner;
    
    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        animationTrigger = stats.dashID;
        afterImageParent = new GameObject("DashAfterImages");
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
    protected override void InstanceAfterImage()
    { 
        StartCoroutine(AfterImageCoroutine());
    }

    private IEnumerator AfterImageCoroutine()
    {
        while (true) {
            
            yield return new WaitForSeconds(0.020f);

            Quaternion quaternion = direction.x != 1 ? Quaternion.Euler(0f, 180f, 0f) : Quaternion.identity;
            Vector3 pos =  this.gameObject.transform.position - new Vector3(0.15f * controller.facingDirection, 0 , 0);
            visualEffectSpawner.InstanceEffect(afterImageParent, pos, quaternion, stateIndex, animationIndex);
        }
    }
    protected override void ChangeAnimationIndex() 
    {
    }

    private void OnDestroy() {
        Destroy(afterImageParent);
    }
}
