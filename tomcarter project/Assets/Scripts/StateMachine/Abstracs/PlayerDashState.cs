using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerDashState : PlayerSkillState
{
    protected float lastDashTime;
    protected Vector2 direction;
    protected bool coyoteTime;
    protected bool wastedCoyoteTime;
    protected float coyoteStartTime;
    protected float currentSpeed;
    protected override void DoChecks()
    {
        base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
        controller.SetTotalVelocity(currentSpeed,direction);
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        controller.SetTotalVelocity(0,Vector2.zero);
        controller.SetAcceleration(1);
        controller.SetGravity(false);
        controller.SetDrag(stats.drag);
        coyoteTime = false;
        wastedCoyoteTime = !controller.Grounded();
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
        lastDashTime = Time.time;
        if(direction.x !=0)
        {
            if(inputs.FixedAxis.x == direction.x)
            {
                controller.SetAcceleration(1);
            }
            else
            {
                controller.SetAcceleration(.5f);
            }
        }
    }

    protected override void TransitionChecks()
    {
        base.TransitionChecks();
        if(Physics.Raycast(_target.transform.position, direction,stats.hedgeDetectionLenght, stats.hedge))
        {
            _target.ChangeState<PlayerHedgeState>();
            controller.SetDrag(0);
        }  
        else if(inputs.JumpInput)
        {
            _target.ChangeState<PlayerDashJumpState>();
            inputs.UsedJump();            
        }    
    }
    public bool CanDash() => Time.time >= lastDashTime + stats.dashCooldown;
}
