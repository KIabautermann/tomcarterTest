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
        Vector3 direction = controller.CurrentVelocity.normalized;
        base.TransitionChecks();
        if (controller.Grounded() && !stateDone)
        {
            _target.ChangeState<PlayerLandState>();        
        }
        else if (inputs.HookInput)
        {
            _target.ChangeState<PlayerHookState>();
        }
        else if(Physics.Raycast(_target.transform.position, direction,stats.hedgeDetectionLenght, stats.hedge) && direction.y > 0)
        {
            _target.ChangeState<PlayerHedgeState>();
            controller.SetTotalVelocity(0,Vector3.zero);
            controller.Force(direction, stats.hedgeTransitionInPush);       
            controller.SetAcceleration(1);
            controller.SetDrag(0);
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
