using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChargedRangeState : PlayerAttackState
{
     Vector3 initialVelocity;
    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);       
        startupTime = 0;
        hitboxTime = stats.rangeHitboxTime;
        recoveryTime = stats.rangerecoveryTime;
        hitbox = stats.chargedRangeHitbox;
        hitboxOffset = stats.chargedRangeHiboxOffset;
        animationTrigger = stats.longRangeTrigger;
    }
    protected override void DoChecks()
    {
        base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();   
        controller.Accelerate(-1/stats.groundedAccelerationTime * Time.deltaTime); 
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        controller.SetTotalVelocity(.5f, controller.CurrentVelocity.normalized);
        if(inputs.FixedAxis.x !=0){
            controller.SetVelocityX(stats.rangeInitialImpulse * controller.facingDirection);
        }
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
        controller.SetGravity(true);
        controller.SetAcceleration(inputs.FixedAxis.x != 0 ? 1 : 0f);
    }

    protected override void TransitionChecks()
    {
        base.TransitionChecks();
    }

    private void OnDrawGizmos() {
        if(activeHitbox){
            Vector3 offset = new Vector3(hitboxOffset.x * controller.facingDirection, hitboxOffset.y,0);
            Gizmos.DrawWireCube(transform.position + offset, hitbox);
        }       
    }
}
