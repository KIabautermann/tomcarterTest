using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHedgeState : PlayerSkillState
{
    private Vector3 direction;
    private bool exitingHedge;

    protected override void DoChecks()
    {
         base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
        if(!exitingHedge)
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
            Vector3 checkPosition = transform.position + direction * stats.hedgeDetectionOffset;
            Collider[] check = Physics.OverlapBox(checkPosition, controller.myCollider.bounds.size/2, Quaternion.identity,stats.hedge);  
            if(check.Length == 0)
            {
                exitingHedge = true;
                controller.SetTotalVelocity(0, Vector3.zero);
                controller.Force(direction,stats.hedgeTransitionOutPush);
            }  
        }
    }

    protected override void DoTransitionIn()
    {
        exitingHedge = false;
        base.DoTransitionIn();
        direction=Vector2.right * controller.facingDirection;
        controller.SetGravity(false);
        direction = controller.CurrentVelocity.normalized;
        Physics.IgnoreLayerCollision(9,10,true);
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
        controller.SetGravity(true);
        Physics.IgnoreLayerCollision(9,10,false);
        Debug.Log(coolDown);
    }

    protected override void TransitionChecks()
    {
        base.TransitionChecks(); 
        if(exitingHedge)
        {
            Collider[] check = Physics.OverlapBox(transform.position, controller.myCollider.bounds.size/2, Quaternion.identity,stats.hedge);  
            if(check.Length == 0)
            {
                if(inputs.FixedAxis.x != 0)
                {
                    controller.SetAcceleration(1);
                }
                else
                {
                 controller.SetAcceleration(0);
                }
                stateDone = true;
            }  
        }  
        
    } 

    private void OnDrawGizmos() 
    {
        Gizmos.DrawWireCube((transform.position + direction*stats.hedgeDetectionOffset), controller.myCollider.bounds.size);
        Gizmos.DrawWireCube(transform.position, controller.myCollider.bounds.size);
    }
}
