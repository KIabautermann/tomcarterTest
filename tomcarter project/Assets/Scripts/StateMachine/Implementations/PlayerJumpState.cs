using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerSkillState
{ 
    private bool _dashJumpCoyoteTime;
    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        animationTrigger = stats.jumpTrigger;
    }
   
   protected override void DoChecks()
    {
        base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();     
        controller.FlipCheck(inputs.FixedAxis.x);
        controller.Accelerate((inputs.FixedAxis.x != 0 ? 1 / stats.airAccelerationTime : -1 / stats.airAccelerationTime) * Time.deltaTime); 
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
        controller.SetVelocityX(stats.movementVelocity * controller.lastDirection);      
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        inputs.UsedJump();
        controller.SetVelocityY(stats.jumpVelocity);
        _dashJumpCoyoteTime = true;
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
    }

    protected override void TransitionChecks()
    {
        if(inputs.JumpCancel){
            controller.SetVelocityY(controller.CurrentVelocity.y * stats.shortHopMultiplier);
            stateDone = true;
        }
        else if(controller.CurrentVelocity.y <=0){
            stateDone = true;
        }      
        else if (controller.Grounded())
        {
            _target.ChangeState<PlayerLandState>();
        }
        else if (inputs.DashInput && _dashJumpCoyoteTime)
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
            _target.ChangeState<PlayerRangeState>();
            inputs.UsedRange();
        }
        base.TransitionChecks();
    }

    private void DashJumpCoyoteTimeCheck()
    {
        if (_dashJumpCoyoteTime && counter > stats.jumpHandicapTime)
        {
            _dashJumpCoyoteTime = false;
        }
    }
}
