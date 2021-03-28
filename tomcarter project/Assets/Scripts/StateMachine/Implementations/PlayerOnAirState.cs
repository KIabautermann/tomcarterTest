using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnAirState : PlayerState
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
        
    }

    protected override void DoTransitionOut()
    {
        
    }

    protected override void TransitionChecks()
    {
        if(inputs.DashInput){
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
    }
}
