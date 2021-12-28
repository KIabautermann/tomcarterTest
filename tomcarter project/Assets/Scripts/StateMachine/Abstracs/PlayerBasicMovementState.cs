using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicMovementState : PlayerState
{
    protected float currentSpeed;
    protected float currentAcceleration;
    protected bool canShortHop;
    protected bool canMove;
    protected float fallMultiplier;
    protected bool shortHoped;
    protected PlayerBaseDashState dashReference;
    protected PlayerBlinkDashState blinkReference;
    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        if (GetComponent<PlayerBaseDashState>() != null) dashReference = GetComponent<PlayerBaseDashState>();
        if (GetComponent<PlayerBlinkDashState>() != null) blinkReference = GetComponent<PlayerBlinkDashState>();
    }

    protected bool FallException()
    {
        return dashReference.fallException || blinkReference.fallException;
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
            controller.Accelerate((-1 / currentAcceleration) * Time.deltaTime);
        }
        
        if (inputs.JumpCancel && controller.CurrentVelocity.y > stats.minJumpVelocity && canShortHop && !controller.Grounded() && !shortHoped)
        {
            controller.SetVelocityY(controller.CurrentVelocity.y * stats.shortHopMultiplier);
            shortHoped = true;
        }
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
        if (controller.CurrentVelocity.y <= 0 && !controller.Grounded() && controller.usingGravity && !FallException())
        {
            controller.Force(Physics.gravity.normalized, fallMultiplier, ForceMode.Force);
        }
        else if (FallException())
        {
            controller.Force(-Physics.gravity.normalized, Physics.gravity.magnitude/2, ForceMode.Force);
        }
        controller.SetVelocityX(currentSpeed * controller.lastDirection);
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        canShortHop = true;
        shortHoped = false;
        canMove = true;
        fallMultiplier = stats.fallMultiplier;
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
