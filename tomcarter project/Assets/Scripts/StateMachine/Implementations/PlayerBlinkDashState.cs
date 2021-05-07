using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlinkDashState : PlayerDashState
{
    private bool hardenRequest;
    private bool readyToHarden;
    private float hardenCounter;

    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        animationTrigger = stats.blinkTrigger;
    }
    protected override void DoChecks()
    {
        base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
        if((inputs.GuardInput || !inputs.GuardCancel) && !hardenRequest && !controller.Grounded() && !StartedDash()){
            hardenRequest = true;
            controller.SetAcceleration(0);
            hardenCounter = Time.time;
            inputs.UsedGuard();
        }
        if(hardenRequest){
            HardenSetout();
        }
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        hardenRequest = false;
        readyToHarden = false;
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
        if(counter >= stats.blinkDashLenght && !hardenRequest)
        {
            stateDone = true;
            controller.SetDrag(0);
            controller.SetGravity(true);
        }
        if(readyToHarden)
        {
            controller.SetDrag(0);
            controller.SetGravity(true);
            controller.SetAcceleration(1);
            controller.SetVelocityX(20 * controller.facingDirection);
            _target.ChangeState<PlayerHardenState>();
        }
    }

    private void HardenSetout()
    {
        controller.Accelerate(-1 / stats.airAccelerationTime * Time.deltaTime);
        if(Time.time >= hardenCounter + stats.hardenDashChannelingTime){
            readyToHarden = true;
        }
    }
}
