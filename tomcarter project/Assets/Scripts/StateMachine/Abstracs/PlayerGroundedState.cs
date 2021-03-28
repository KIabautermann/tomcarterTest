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
       
    }

    protected override void DoPhysicsUpdate()
    {
        
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        isExiting = false;
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
        isExiting = true;
    }

    protected override void TransitionChecks()
    {
        if(inputs.JumpInput){
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
        else if(inputs.RootsInput){
            _target.ChangeState<PlayerRootsState>();
            inputs.UsedRoots();
        }
    }
}
