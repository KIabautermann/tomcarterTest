using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRangeState : PlayerAttackState
{
    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        attackDuration = stats.rangeTime;
        animationTrigger = stats.rangeID;
    }

    protected override void DoChecks()
    {
        base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
        if (onAir)
        {
            if (!activeHitbox)
            {
                controller.Accelerate((inputs.FixedAxis.x != 0 ? 1 / stats.airAccelerationTime : -1 / stats.airAccelerationTime) * Time.deltaTime);
                if (inputs.JumpCancel && controller.CurrentVelocity.y >= 0 && !forceAplied)
                {
                    controller.SetVelocityY(controller.CurrentVelocity.y * stats.shortHopMultiplier);
                    forceAplied = true;
                }
            }
            else
            {
                controller.SetTotalVelocity(0, Vector2.zero);
                controller.SetGravity(false);
                controller.Accelerate(0);
            }
            
        }
        else
        {
            controller.Accelerate(-1 / stats.groundedAccelerationTime);
        }
        controller.FlipCheck(inputs.FixedAxis.x);
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
        if (controller.CurrentVelocity.y <= stats.minJumpVelocity && !controller.Grounded())
        {
            controller.Force(Physics.gravity.normalized, stats.fallMultiplier, ForceMode.Force);
        }
        if (!activeHitbox)
        {
            controller.SetVelocityX(stats.movementVelocity * controller.lastDirection);
        }
    }      

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        if (!onAir)
        {
            _target.QueueAnimation(_target.animations.attackRange.name, false, true);
        }
        else
        {
            if(inputs.FixedAxis.y == 0)
            {
                _target.QueueAnimation(_target.animations.attackRange.name, false, true);
            }
            else
            {
                if(inputs.FixedAxis.y > 0)
                {
                    _target.QueueAnimation(_target.animations.attackRangeUp.name, false, true);
                }
                else
                {
                    _target.QueueAnimation(_target.animations.attackRangeDown.name, false, true);
                }
            }
            
        }
    }
    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
        controller.LockFlip(false);
        controller.SetGravity(true);
    }

    protected override void TransitionChecks()
    {
        base.TransitionChecks();
    }

    private void OnDrawGizmos()
    {
        
    }
}
