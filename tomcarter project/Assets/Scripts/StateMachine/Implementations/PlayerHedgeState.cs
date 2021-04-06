using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHedgeState : PlayerSkillState
{
    private Vector2 direction;
    protected override void DoChecks()
    {
         base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
        if(inputs.FixedAxis.magnitude !=0)
        {
            direction = inputs.FixedAxis;
            direction = direction.normalized;
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
        controller.SetGravity(false);
        direction = controller.CurrentVelocity.normalized;
        Physics.IgnoreLayerCollision(9,10,true);
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
        controller.SetGravity(true);
        Physics.IgnoreLayerCollision(9,10,false);
    }

    protected override void TransitionChecks()
    {
        base.TransitionChecks();
        Vector3 detection = controller.myCollider.bounds.size;
        Collider[] hedgeDetection = Physics.OverlapBox(_target.transform.position, detection/2, Quaternion.identity, stats.hedge);
        if(hedgeDetection.Length == 0)
        {
            stateDone = true;
            if(inputs.FixedAxis.x == 0)
            {
                controller.SetAcceleration(0);
            }
            else
            {
                controller.SetAcceleration(1);
            }
        }
    }

     
}
