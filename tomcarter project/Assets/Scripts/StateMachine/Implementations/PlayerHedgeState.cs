using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHedgeState : PlayerSkillState
{
    private Vector3 direction;
    private bool exitingHedge;
    private bool enteringHedge;

    protected override void DoChecks()
    {
         base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
        if(enteringHedge)
        {
            controller.Accelerate(-1 / stats.airAccelerationTime * Time.deltaTime);
            if(controller.CurrentVelocity.magnitude <=.1f)
            {
                enteringHedge = false;
            }
        }
        if(!exitingHedge && !enteringHedge)
        {
            if(inputs.FixedAxis.magnitude !=0)
            {
                direction = new Vector3(inputs.FixedAxis.x, inputs.FixedAxis.y,0).normalized;
            }     
            controller.FlipCheck(inputs.FixedAxis.x);
            controller.Accelerate((inputs.FixedAxis.magnitude != 0 ? 1 / stats.groundedAccelerationTime : -1 / stats.groundedAccelerationTime) * Time.deltaTime);
        }       
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
        if(!exitingHedge)
        {
            controller.SetTotalVelocity(stats.movementVelocity, direction);          
        }
        Vector3 checkDirection = controller.CurrentVelocity.normalized;
        Vector3 checkPosition = transform.position + checkDirection * stats.hedgeDetectionOffset;
        Collider[] check = Physics.OverlapBox(checkPosition, controller.myCollider.bounds.size/2, Quaternion.identity,stats.hedge);  
        if(check.Length == 0)
        {
            exitingHedge = true;
            controller.SetTotalVelocity(0, Vector3.zero);
            controller.Force(direction,stats.hedgeTransitionOutPush);
        }
        else
        {
             exitingHedge = false;
        }  
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        Physics.IgnoreLayerCollision(9,10,true);
        exitingHedge = false;
        enteringHedge = true;
        controller.SetAcceleration(1);
        direction = controller.CurrentVelocity.normalized;     
        controller.SetGravity(false);       
        controller.Force(direction, stats.hedgeTransitionInPush); 
        Debug.Log(direction);
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
        controller.SetGravity(true);
        Physics.IgnoreLayerCollision(9,10,false);
        if(inputs.FixedAxis.x != 0)
        {
            controller.SetAcceleration(1);
        }
        else
        {
            controller.SetAcceleration(0);
        }
        if(!controller.Grounded())
        {
            airState.JumpCoyoteTimeStart();
            airState.DashJumpCoyoteTimeStart();
        }
    }

    protected override void TransitionChecks()
    {
        base.TransitionChecks(); 
        if(exitingHedge)
        {
            if(inputs.JumpInput)
            {
                _target.ChangeState<PlayerJumpState>();
                inputs.UsedJump();
                airState.DashJumpCoyoteTimeStart();
            }
            else
            {
                Collider[] check = Physics.OverlapBox(transform.position, controller.myCollider.bounds.size/2, Quaternion.identity,stats.hedge);  
                if(check.Length == 0)
                {                  
                    stateDone = true;
                }  
            }           
        }         
    } 
}
