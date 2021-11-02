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
        direction = inputs.FixedAxis.x == 0 ? new Vector2(controller.facingDirection,0) : inputs.FixedAxis.x * Vector2.right;
        _target.QueueAnimation(_target.animations.dash.name, false, true);
    }
    protected override void DoTransitionOut()
    {     
        StopAllCoroutines();     
        base.DoTransitionOut();
        _target.QueueAnimation(_target.animations.dashEnd.name, true, false);
    }
    protected override void TransitionChecks()
    {
        base.TransitionChecks();
    }
    protected override void InstanceAfterImage()
    { 
        //StartCoroutine(AfterImageCoroutine());
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

    private void OnDestroy() {
        Destroy(afterImageParent);
    }
}
