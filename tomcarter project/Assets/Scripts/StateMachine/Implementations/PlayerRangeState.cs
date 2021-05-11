using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRangeState : PlayerAttackState
{

    Vector3 initialVelocity;
    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);       
        coolDown = stats.rangeCooldown;
        startupTime = stats.rangeStartupTime;
        hitboxTime = stats.rangeHitboxTime;
        recoveryTime = stats.rangerecoveryTime;
        hitbox = stats.rangeHitbox;
        hitboxOffset = stats.rangeHiboxOffset;
        animationTrigger = stats.rangeTrigger;
    }
    protected override void DoChecks()
    {
        base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
        if(counter >=  startupTime){
            controller.SetGravity(true);
            controller.SetAcceleration(1f);
            controller.Accelerate((inputs.FixedAxis.x != 0 ? 1 / stats.airAccelerationTime : -1 / stats.airAccelerationTime) * Time.deltaTime);
        }
        else controller.Accelerate(-1 / stats.airAccelerationTime * Time.deltaTime);
        if(counter >=  startupTime + hitboxTime){
            stateDone=true;
        }       
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
        if(counter>=startupTime){
            controller.SetVelocityX(stats.movementVelocity * controller.lastDirection);
        }
        else{
            controller.SetTotalVelocity(initialVelocity.magnitude, initialVelocity.normalized);
        }
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        initialVelocity = controller.CurrentVelocity;
        controller.SetAcceleration(1);
        controller.SetGravity(false);          
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
        controller.SetGravity(true);
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
