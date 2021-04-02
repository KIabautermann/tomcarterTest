using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerGroundedState : PlayerState
{

    protected override void DoChecks()
    {
        
    }

    protected override void DoLogicUpdate()
    {
       controller.SetVelocityX(stats.movementVelocity * controller.facingDirection);
    }

    protected override void DoPhysicsUpdate()
    {
        
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
        if(inputs.RootsInput && !inputs.RootsCancel){
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
            _target.ChangeState<PlayerRangeState>();
            inputs.UsedRange();
        }
        else if(inputs.HookInput){
            _target.ChangeState<PlayerHookState>();
            inputs.UsedHook();
        }
        else if(inputs.GuardInput){
            _target.ChangeState<PlayerGuardState>();
            inputs.UsedGuard();
        }

        base.TransitionChecks();
    }
}
