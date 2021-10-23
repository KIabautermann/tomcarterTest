using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementState : PlayerGroundedState
{
 
 
    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        animationTrigger = stats.movementID;
        stateIndex = stats.runNumberID;
    }

    protected override void DoChecks()
    {
         base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        controller.FlipCheck(inputs.FixedAxis.x);
        base.DoLogicUpdate();
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        _target.QueueAnimation(_target.animations.runInit.name, false,false);
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
    }

    protected override void TransitionChecks()
    {
       if(inputs.FixedAxis.x == 0)
        {
           _target.ChangeState<PlayerIdleState>();
        }
        base.TransitionChecks();
    }
}
