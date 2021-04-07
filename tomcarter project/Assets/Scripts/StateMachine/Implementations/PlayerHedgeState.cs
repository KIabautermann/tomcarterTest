using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHedgeState : PlayerSkillState
{
    private Vector3 direction;


    protected override void DoChecks()
    {
         base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
        if(inputs.FixedAxis.magnitude !=0)
        {
            direction = new Vector3(inputs.FixedAxis.x, inputs.FixedAxis.y,0).normalized;
        }     
        controller.FlipCheck(inputs.FixedAxis.x);
        controller.Accelerate((inputs.FixedAxis.magnitude != 0 ? 1 / stats.groundedAccelerationTime : -1 / stats.groundedAccelerationTime) * Time.deltaTime);
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
        controller.SetTotalVelocity(stats.movementVelocity, direction);
    }

    protected override void DoTransitionIn()
    {
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
        Vector3 checkPosition = transform.position + (direction*stats.hedgeDetectionOffset);
        Collider[] check = Physics.OverlapBox(checkPosition, controller.myCollider.bounds.size, Quaternion.identity,stats.hedge);  
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
