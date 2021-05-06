using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeState : PlayerAttackState
{
    private bool _falling;
    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        startupTime = stats.meleeStartupTime;
        hitboxTime = stats.meleeHitboxTime;
        recoveryTime = stats.meleerecoveryTime;
        hitbox = stats.meleeHitbox;
        hitboxOffset = stats.meleeHiboxOffset;
        animationTrigger = stats.meleeTrigger;
    }

    
    protected override void DoChecks()
    {
        base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
        controller.DirectionCheck(inputs.FixedAxis.x);
        if(!chainAttack && inputs.MeleeInput && Time.time >= startTime + startupTime + hitboxTime){
            chainAttack = true;
        }  
        if(!controller.Grounded()){
            SetJumpVelocity();
        }
        else{
            if(inputs.JumpInput){
            controller.SetVelocityY(stats.jumpVelocity);
            inputs.UsedJump();
            }
        }
        controller.Accelerate((inputs.FixedAxis.x != 0 ? 1 / stats.airAccelerationTime : -1 / stats.airAccelerationTime) * Time.deltaTime);
        controller.SetVelocityX(stats.movementVelocity * inputs.FixedAxis.x);  
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
        /*if(controller.CurrentVelocity.y < stats.minJumpVelocity && !controller.Grounded())
        {
            controller.Force(Physics.gravity.normalized,Physics.gravity.magnitude * stats.fallMultiplier, ForceMode.Force);
        }*/
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        _falling = controller.CurrentVelocity.y < 0;
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
        controller.FlipCheck(inputs.FixedAxis.x);
    }

    protected override void TransitionChecks()
    {
        base.TransitionChecks();  
        if(Time.time >= startTime + startupTime + hitboxTime + recoveryTime && chainAttack){
            _target.ChangeState<PlayerMeleeState>();
        }   
    }

    private void SetJumpVelocity()
    {
        if (controller.CurrentVelocity.y > 0)
        {
            if (inputs.JumpCancel && !_falling)
            {
                controller.SetVelocityY(controller.CurrentVelocity.y * stats.shortHopMultiplier);
                _falling = true;
            }
        }
    }

    private void OnDrawGizmos() {
        if(activeHitbox){
            Vector3 offset = stats.meleeHiboxOffset;
            Gizmos.DrawWireCube(transform.position + offset, hitbox);
        }
    }
}
