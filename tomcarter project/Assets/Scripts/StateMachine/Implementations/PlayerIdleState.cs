using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
   
   public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        animationTrigger = stats.idleID;
        stateIndex = stats.idleNumberID;
    
    }
    protected override void DoChecks()
    {
         base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        controller.Accelerate(-1 / stats.groundedAccelerationTime * Time.deltaTime);  
        base.DoLogicUpdate();
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        controller.SetCollider(stats.colliderDefaultSize, stats.colliderDefaultPosition);
        _target.QueueAnimation(_target.animations.idle.name, false, false);
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
