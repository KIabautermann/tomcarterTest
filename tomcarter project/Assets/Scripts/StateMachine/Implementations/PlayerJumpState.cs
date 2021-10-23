using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBasicMovementState
{ 
    private bool _jumped;
    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        animationTrigger = stats.jumpID;
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
        platformManager.LogicUpdated();
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();     
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        inputs.UsedJump();
        _target.QueueAnimation(_target.animations.airJump.name, false, false);
        controller.SetVelocityY(stats.jumpVelocity);
        currentAcceleration = stats.airAccelerationTime;
        currentSpeed = stats.movementVelocity;
    }

    protected override void DoTransitionOut()
    {
        platformManager.LogicExit();
        base.DoTransitionOut();
    }

    protected override void TransitionChecks()
    {
        if (inputs.DashInput) 
        {       
            if(counter<= stats.dashCoyoteTime)
            {
                _target.ChangeState<PlayerDashJumpState>();
            }
            else
            {
                _target.ChangeState<PlayerDashState>();
            }
            inputs.UsedDash();          
        }
        else if(inputs.JumpCancel){
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
}
