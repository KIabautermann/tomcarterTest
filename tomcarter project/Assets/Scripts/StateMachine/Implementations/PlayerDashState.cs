using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerSkillState
{
    protected override void DoChecks()
    {
         base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
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
        if(inputs.JumpInput)
        {
            _target.ChangeState<PlayerDashJumpState>();
            inputs.UsedJump();
        }
        else if(inputs.GuardInput){
            _target.ChangeState<PlayerGuardState>();
            inputs.UsedGuard();
        }
        else
        {
            base.TransitionChecks();
        }       
    }
}
