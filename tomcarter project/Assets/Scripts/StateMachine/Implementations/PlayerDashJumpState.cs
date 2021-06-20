using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashJumpState : PlayerSkillState
{
    private bool _isJumping;
    
    
    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        animationTrigger = stats.dashJumpID;
    }
    protected override void DoChecks()
    {
        base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
        platformManager.LogicUpdated();
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
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
        if (controller.CurrentVelocity.y < stats.minDashJumpVelocity)
        {
            controller.Force(Physics.gravity.normalized, stats.dashJumpFallMultiplier, ForceMode.Force);
        }
        controller.SetVelocityX(stats.dashJumpVelocityX * controller.lastDirection);
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
        platformManager.LogicExit();
        base.DoTransitionOut();
    }

    protected override void TransitionChecks()
    {
        Vector3 direction = controller.CurrentVelocity.normalized;
        base.TransitionChecks();
        if(Physics.Raycast(_target.transform.position, direction,stats.collisionDetection, stats.hedge))
        {
            _target.ChangeState<PlayerHedgeState>();             
        }
        else if (inputs.HookInput)
        {
            _target.ChangeState<PlayerHookState>();
            inputs.UsedHook();
        }
        else if(inputs.RangeInput){
            _target.ChangeState<PlayerRangeState>();
            inputs.UsedRange();
        }
        else if (controller.Grounded() && !stateDone)
        {
            _target.ChangeState<PlayerLandState>();        
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
