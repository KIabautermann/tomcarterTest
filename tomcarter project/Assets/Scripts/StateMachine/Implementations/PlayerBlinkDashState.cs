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
        animationTrigger = stats.blinkID;
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
        if(direction.y == 0){
            animationIndex = 2;
        } 
        else if(direction.x == 0){
            if(direction.y>0){
                animationIndex = 3;
            }
            else{
                animationIndex = 4;
            }
        }
        else{
            if(direction.y>0){
                animationIndex = 5;
            }
            else{
                animationIndex = 6;
            }         
        }  
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
        
        controller.SetGravity(true);
        controller.SetDrag(0);
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
    protected override void TransitionChecks()
    {
        base.TransitionChecks();
        if(counter >= stats.blinkDashLenght && !hardenRequest && !stateDone)
        {
            stateDone = true;
        }
        if(readyToHarden)
        {
            controller.SetAcceleration(1);
            controller.SetVelocityX(stats.hardenDashVelocity * controller.lastDirection);
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
