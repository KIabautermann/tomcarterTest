using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRangeState : PlayerAttackState
{

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
            controller.SetAcceleration(.25f);
            if(controller.CurrentVelocity.y < stats.minJumpVelocity && !controller.Grounded())
            {
                controller.Force(Physics.gravity.normalized,stats.fallMultiplier, ForceMode.Force);
            }
            controller.Accelerate((inputs.FixedAxis.x != 0 ? 1 / stats.airAccelerationTime : -1 / stats.airAccelerationTime) * Time.deltaTime);
        }
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
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        controller.SetGravity(false);    
        controller.SetTotalVelocity(stats.rangeMovementSpeed, controller.CurrentVelocity.normalized);   
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
}
