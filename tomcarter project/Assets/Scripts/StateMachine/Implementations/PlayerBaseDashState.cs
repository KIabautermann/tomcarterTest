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
    }
    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
    }
    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        currentSpeed = stats.dashSpeed;
        direction = new Vector2(controller.facingDirection,0);
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
            stateDone = true;
            controller.SetDrag(0);
            controller.SetGravity(true);
        }
    }
}
