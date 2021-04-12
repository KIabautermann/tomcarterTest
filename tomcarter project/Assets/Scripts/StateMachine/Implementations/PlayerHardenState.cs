using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHardenState : PlayerSkillState
{
    public override string ToString()
    {
        return base.ToString();
    }

    protected override void DoChecks()
    {
        base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
        if(controller.Grounded())
        {
            controller.FlipCheck(inputs.FixedAxis.x);
            controller.Accelerate((inputs.FixedAxis.x != 0 ? 1 / stats.airAccelerationTime : -1 / stats.airAccelerationTime) * Time.deltaTime);
            controller.SetVelocityX(stats.hardenMovementSpeed * controller.facingDirection);                    
        }   
        RaycastHit hit;
            if(Physics.Raycast(transform.position, controller.CurrentVelocity.normalized,out hit, 1))
            {
                if (hit.collider.gameObject.GetComponent<IBreakable>() != null)
                {
                    hit.collider.gameObject.GetComponent<IBreakable>().onBreak(controller.CurrentVelocity);     
                    stateDone = true;               
                }                
            }                   
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
        base.TransitionChecks();
        if(Time.time >= startTime + stats.hardenTime)
        {
            stateDone = true;
        }
    }
}
