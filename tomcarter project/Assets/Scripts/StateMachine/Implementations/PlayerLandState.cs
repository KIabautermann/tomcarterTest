using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerTransientState
{
    
    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        animationTrigger = stats.landID;
        stateIndex = stats.airNumberID;
    }
    
    protected override void DoChecks()
    {
        base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
        controller.Accelerate((inputs.FixedAxis.x != 0 ? 1 / stats.groundedAccelerationTime : -1 / stats.groundedAccelerationTime) * Time.deltaTime);
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
        controller.SetVelocityX(stats.movementVelocity * controller.lastDirection);
    }

    protected override void DoTransitionIn()
    {
        PlayerEventSystem.GetInstance().TriggerPlayerHasLanded(transform.position);
        stateDone = false;
        animationIndex = 4;
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
    }

    protected override void TransitionChecks()
    {
        base.TransitionChecks();
        if(controller.OnRootable() && inputs.RootsInput){
            _target.ChangeState<PlayerRootsState>();
            inputs.UsedRoots();
        }
        else if(inputs.JumpInput){
            _target.ChangeState<PlayerJumpState>();
            inputs.UsedJump();     
        }
        else if(inputs.DashInput){
            _target.ChangeState<PlayerDashState>();
            inputs.UsedDash();     
        }
        else if(inputs.MeleeInput){
            _target.ChangeState<PlayerMeleeState>();
            inputs.UsedMelee();
        }
        else if(inputs.RangeInput){
            _target.ChangeState<PlayerRangeChargeState>();
            inputs.UsedRange();
        }
        else if(inputs.HookInput){
            _target.ChangeState<PlayerHookState>();
            inputs.UsedHook();
        }
        else if(inputs.GuardInput)
        {
            _target.ChangeState<PlayerHardenState>();
            inputs.UsedGuard();
        }
        else if(!controller.Grounded())
        {
            _target.ChangeState<PlayerOnAirState>();
            airState.JumpCoyoteTimeStart();
        }
        else if(endByAnimation){
            stateDone = true;
        }

        base.TransitionChecks();
    }
}
