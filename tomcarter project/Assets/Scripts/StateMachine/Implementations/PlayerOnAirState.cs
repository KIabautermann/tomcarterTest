using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnAirState : PlayerBasicMovementState
{
    private bool _jumpCoyoteTime;

    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        animationTrigger = stats.airID;
        stateIndex = stats.airNumberID;
    }

    protected override void DoChecks()
    {
       base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
        controller.FlipCheck(inputs.FixedAxis.x);       
        JumpCoyoteTimeCheck();
    }
    
    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        _target.QueueAnimation(_target.animations.airPeak.name, false, false);
        currentAcceleration = stats.airAccelerationTime;
        currentSpeed = stats.movementVelocity;
        canShortHop = false;
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
            _jumpCoyoteTime = false;
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
           _target.ChangeState<PlayerRangeState>();
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
