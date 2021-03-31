using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnAirState : PlayerState
{
    private bool _isJumping;
    private bool _jumpCoyoteTime;
    private bool _dashJumpCoyoteTime;
    protected override void DoChecks()
    {
       base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        controller.FlipCheck(inputs.FixedAxis.x);
        controller.Accelerate((inputs.FixedAxis.x != 0 ? 1 / stats.airAccelerationTime : -1 / stats.airAccelerationTime) * Time.deltaTime);
        controller.SetVelocityX(stats.movementVelocity * controller.facingDirection);
        JumpCoyoteTimeCheck();
        DashJumpCoyoteTimeCheck();
        SetJumpVelocity();
        ClampYVelocity();
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
        if(controller.CurrentVelocity.y < stats.minJumpVelocity)
        {
            controller.Force(Physics.gravity, stats.fallMultiplier);
        }
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
    }

    protected override void TransitionChecks()
    {
        if (grounded && controller.CurrentVelocity.y < .01f)
        {
            _target.ChangeState<PlayerLandState>();
        }
        else if (inputs.JumpInput && _jumpCoyoteTime)
        {
            _target.ChangeState<PlayerJumpState>();
        }
        else if (inputs.DashInput)
        {
            if(_dashJumpCoyoteTime)
            {
                _target.ChangeState<PlayerDashJumpState>();
            }
            else
            {
               _target.ChangeState<PlayerBaseDashState>();
            }            
        }
        else if (inputs.HookInput)
        {
            _target.ChangeState<PlayerHookState>();
        }

        base.TransitionChecks();
    }

    private void JumpCoyoteTimeCheck()
    {
        if(_jumpCoyoteTime && Time.time > startTime + stats.coyoteTime)
        {
            _jumpCoyoteTime = false;
        }
    }
    private void DashJumpCoyoteTimeCheck()
    {
        if (_dashJumpCoyoteTime && Time.time > startTime + stats.jumpHandicapTime)
        {
            _dashJumpCoyoteTime = false;
        }
    }
    private void SetJumpVelocity()
    {
        if (_isJumping)
        {
            if (inputs.JumpCancel)
            {
                controller.SetVelocityY(controller.CurrentVelocity.y * stats.shortHopMultiplier);
                _isJumping = false;
            }
            else if(controller.CurrentVelocity.y <= 0)
            {
                _isJumping = false;
            }
        }
    }
    
    public void JumpCoyoteTimeStart() => _jumpCoyoteTime = true;

    public void DashJumpCoyoteTimeStart() => _dashJumpCoyoteTime = true;

    public void SetJump() => _isJumping = true;
}
