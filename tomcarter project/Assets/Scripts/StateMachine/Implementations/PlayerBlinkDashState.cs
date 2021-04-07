using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlinkDashState : PlayerDashState
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
        currentSpeed = stats.blinkDashSpeed;
        if(inputs.FixedAxis != Vector2.zero)
        {
            direction = inputs.FixedAxis;
            direction = direction.normalized;
        }
        else
        {
            direction = new Vector2(controller.facingDirection,0);
        }     
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
        if(stateDone)
        {
            if (controller.CurrentVelocity.y > 0)
            {
                controller.SetVelocityY(controller.CurrentVelocity.y * stats.dashEndMultiplier);
            }
            if(controller.CurrentVelocity.x != 0)
            {
                controller.SetAcceleration(1);
            }
            else
            {
                controller.SetAcceleration(0);
            }
        }       
    }
    protected override void TransitionChecks()
    {
        base.TransitionChecks();
        if(Time.time > startTime + stats.blinkDashLenght)
        {
            stateDone = true;
            controller.SetDrag(0);
            controller.SetGravity(true);
        }
    }
}
