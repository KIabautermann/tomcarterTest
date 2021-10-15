using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeState : PlayerAttackState
{
    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        attackDuration = stats.meleeTime;
        animationTrigger = stats.meleeID;
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
            controller.Accelerate((inputs.FixedAxis.x != 0 ? 1 / stats.airAccelerationTime : -1 / stats.airAccelerationTime) * Time.deltaTime);
            if (inputs.JumpCancel && controller.CurrentVelocity.y >= 0 && !forceAplied)
            {
                controller.SetVelocityY(controller.CurrentVelocity.y * stats.shortHopMultiplier);
                forceAplied = true;
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
        controller.SetVelocityX(stats.movementVelocity * controller.lastDirection);
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        if (!onAir)
        {
            _target.QueueAnimation(_target.animations.attackGround.name, false, true);
        }
        else
        {
            _target.QueueAnimation(_target.animations.attackAir.name, false, true);
        }
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
        controller.LockFlip(false);
    }

    protected override void TransitionChecks()
    {
        base.TransitionChecks();
        if(onAir && controller.Grounded())
        {
            _target.ChangeState<PlayerLandState>();
        }
    }
}
