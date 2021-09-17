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
        _target.QueueAnimation(_target.animations.airLand.name, true, true);
        stateDone = true;
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
    }

    
}
