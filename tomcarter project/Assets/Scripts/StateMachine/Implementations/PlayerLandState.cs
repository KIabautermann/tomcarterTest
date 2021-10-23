using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerGroundedState
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
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        PlayerEventSystem.GetInstance().TriggerPlayerHasLanded(transform.position);
        _target.QueueAnimation(_target.animations.airLand.name, true, true);
        stateDone = true;
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
        controller.SetVelocityY(0);
    }

    
}
