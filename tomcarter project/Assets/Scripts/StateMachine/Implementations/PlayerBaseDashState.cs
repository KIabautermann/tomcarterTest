using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseDashState : PlayerDashState
{  
    protected override void DoChecks()
    {
        base.DoChecks();
    }
    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
        controller.SetVelocityX(controller.facingDirection * stats.dashSpeed);
    }
    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
    }
    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        controller.SetVelocityX(controller.facingDirection * stats.dashSpeed);
    }
    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
    }
    protected override void TransitionChecks()
    {
        base.TransitionChecks();
        if(Time.time > startTime + stats.dashLenght)
        {
            abilityDone = true;
            controller.SetDrag(0);
            controller.SetGravity(true);
            controller.SetAcceleration(1);
        }
    }
}
