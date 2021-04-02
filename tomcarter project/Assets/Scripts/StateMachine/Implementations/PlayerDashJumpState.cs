using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashJumpState : PlayerSkillState
{
    private bool _isJumping;
    protected override void DoChecks()
    {
         base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
        setJumpVelocity();
        controller.FlipCheck(inputs.FixedAxis.x);
        if (inputs.FixedAxis.x == 0)
        {
            controller.Accelerate(-1f / stats.dashJumpAccelerationTime * Time.deltaTime);
        }
        else
        {
            controller.Accelerate(1f / stats.dashJumpAccelerationTime * Time.deltaTime);
        }

        controller.SetVelocityX(stats.dashJumpVelocityX * controller.facingDirection);
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
        if (controller.CurrentVelocity.y < stats.minDashJumpVelocity)
        {
            controller.Force(Physics.gravity, stats.dashJumpFallMultiplier);
        }
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        _isJumping = true;
        controller.SetDrag(0);
        controller.SetGravity(true);
        controller.SetAcceleration(Mathf.Abs(inputs.FixedAxis.x));
        controller.SetVelocityX(stats.dashJumpVelocityX * controller.facingDirection);
        controller.SetVelocityY(stats.jumpVelocity);
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
    }

    protected override void TransitionChecks()
    {
        base.TransitionChecks();
        if (grounded && !stateDone)
        {
            _target.ChangeState<PlayerLandState>();        
        }
        else if (inputs.HookInput)
        {
            _target.ChangeState<PlayerHookState>();
        }
    }

    private void setJumpVelocity()
    {
        if (_isJumping)
        {
            if (inputs.JumpCancel)
            {
                controller.SetVelocityY(controller.CurrentVelocity.y * stats.shortHopMultiplier);
                _isJumping = false;
            }
            else if (controller.CurrentVelocity.y <= 0)
            {
                _isJumping = false;
            }
        }
    }
}
