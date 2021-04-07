using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerDashState : PlayerSkillState
{
    protected float lastDashTime;
    protected Vector2 direction;
    protected bool coyoteTime;
    protected float coyoteStartTime;
    protected float currentSpeed;
    protected bool dashJumpCoyoteTime;

    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        coolDown = stats.dashCooldown;
    }
    protected override void DoChecks()
    {
        base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
        controller.SetTotalVelocity(currentSpeed,direction);
        DashJumpCoyoteTimeCheck();
        if(controller.Grounded())
        {
            dashJumpCoyoteTime = true;
            coyoteStartTime = Time.time;
        }
        else
        {
            DashJumpCoyoteTimeCheck();
        }
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        inputs.UsedDash();
        controller.SetTotalVelocity(0,Vector2.zero);
        controller.SetAcceleration(1);
        controller.SetGravity(false);
        controller.SetDrag(stats.drag);
        coyoteTime = false;
        dashJumpCoyoteTime = controller.Grounded();
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
            controller.SetTotalVelocity(0,Vector3.zero);
            controller.Force(direction, stats.hedgeTransitionInPush);       
            controller.SetAcceleration(1);
            controller.SetDrag(0);
        }  
        else if(inputs.JumpInput && dashJumpCoyoteTime)
        {
            _target.ChangeState<PlayerDashJumpState>();
            inputs.UsedJump();            
        }    
    }
    public void DashJumpCoyoteTimeStart() => dashJumpCoyoteTime = true;

    private void DashJumpCoyoteTimeCheck()
    {
        if (dashJumpCoyoteTime && Time.time > startTime + stats.jumpHandicapTime)
        {
            dashJumpCoyoteTime = false;
        }
    }
   
    public bool CanDash() => Time.time >= lastDashTime + stats.dashCooldown;
}
