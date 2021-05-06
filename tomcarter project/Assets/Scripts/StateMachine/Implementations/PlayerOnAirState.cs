using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnAirState : PlayerState
{
    private bool _jumpCoyoteTime;
    private bool _dashJumpCoyoteTime;
    private bool _falling;

    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        animationTrigger = stats.airTrigger;
    }

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
    }

    protected override void TransitionChecks()
    {
        if (controller.Grounded())
        {
            _target.ChangeState<PlayerLandState>();
        }
        else if (inputs.JumpInput && _jumpCoyoteTime)
        {
            _target.ChangeState<PlayerJumpState>();
            inputs.UsedJump();
        }
        else if (inputs.DashInput)
        {
            if(_dashJumpCoyoteTime)
            {
                _target.ChangeState<PlayerDashJumpState>();
            }
            else
            {
               _target.ChangeState<PlayerDashState>();
            }  
            inputs.UsedDash();          
        }
        else if (inputs.HookInput)
        {
            _target.ChangeState<PlayerHookState>();
            inputs.UsedHook();
        }
        else if(inputs.MeleeInput){
            _target.ChangeState<PlayerMeleeState>();
            inputs.UsedMelee();
        }
        else if(inputs.GuardInput)
        {
            _target.ChangeState<PlayerHardenState>();
            inputs.UsedGuard();
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
        if (controller.CurrentVelocity.y > 0 && !_falling)
        {
            if (inputs.JumpCancel)
            {
                controller.SetVelocityY(controller.CurrentVelocity.y * stats.shortHopMultiplier);
                _falling=true;
            }            
        }
    }
    
    public void JumpCoyoteTimeStart() => _jumpCoyoteTime = true;

    public void DashJumpCoyoteTimeStart() => _dashJumpCoyoteTime = true;
}
