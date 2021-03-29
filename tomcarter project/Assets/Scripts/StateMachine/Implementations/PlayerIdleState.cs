using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
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
        if(inputs.FixedAxis.x != 0)
        {
            _target.ChangeState<PlayerMovementState>();
        }
        else
        {
            base.TransitionChecks();
        }
    }
}
