using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicMovementState : PlayerState
{
    protected float currentSpeed;
    protected float currentAcceleration;
    protected bool canShortHop;
    protected bool canMove;
    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
        if (canMove)
        {
            controller.Accelerate(((inputs.FixedAxis.x != 0 ? 1 : -1) / currentAcceleration) * Time.deltaTime);
        }
        else
        {
            controller.Accelerate(-1 / currentAcceleration);
        }
        
        if (inputs.JumpCancel && controller.CurrentVelocity.y > stats.minJumpVelocity && canShortHop)
        {
            controller.SetVelocityY(controller.CurrentVelocity.y * stats.shortHopMultiplier);
        }
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
        if (controller.CurrentVelocity.y <= stats.minJumpVelocity && !controller.Grounded() && controller.usingGravity)
        {
            controller.Force(Physics.gravity.normalized, stats.fallMultiplier, ForceMode.Force);
            Debug.Log("a");
        }
        controller.SetVelocityX(currentSpeed * controller.lastDirection);
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        canShortHop = true;
        canMove = true;
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
    }

    protected override void TransitionChecks()
    {
        base.TransitionChecks();
    }
}
