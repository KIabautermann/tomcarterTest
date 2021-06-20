using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnAirState : PlayerState
{
    private bool _jumpCoyoteTime;

    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        animationTrigger = stats.airID;
    }

    protected override void DoChecks()
    {
       base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        controller.FlipCheck(inputs.FixedAxis.x);
        controller.Accelerate((inputs.FixedAxis.x != 0 ? 1 / stats.airAccelerationTime : -1 / stats.airAccelerationTime) * Time.deltaTime);
        
        JumpCoyoteTimeCheck();
    }
    
    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
        if(controller.CurrentVelocity.y <= stats.minJumpVelocity && !controller.Grounded())
        {
            controller.Force(Physics.gravity.normalized,stats.fallMultiplier, ForceMode.Force);
        }
        controller.SetVelocityX(stats.movementVelocity * controller.lastDirection);
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
            _target.ChangeState<PlayerDashState>();          
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
        else if(inputs.RangeInput){
           _target.ChangeState<PlayerRangeChargeState>();
            inputs.UsedRange();
        }

        base.TransitionChecks();
    }

    private void JumpCoyoteTimeCheck()
    {
        if(_jumpCoyoteTime && counter > + stats.coyoteTime)
        {
            _jumpCoyoteTime = false;
        }
    }
    
    
    public void JumpCoyoteTimeStart() => _jumpCoyoteTime = true;

}
