using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBasicMovementState
{ 
    private bool _jumped;
    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        animationTrigger = stats.jumpID;
        stateIndex = stats.airNumberID;
    }
   
   protected override void DoChecks()
   {
        base.DoChecks();
   }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();  
        controller.FlipCheck(inputs.FixedAxis.x);
        platformManager.LogicUpdated();
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();     
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        inputs.UsedJump();
        _target.QueueAnimation(_target.animations.airUpwards.name, false, true);
        currentAcceleration = stats.airAccelerationTime;
        currentSpeed = stats.movementVelocity;
        
        // La velocidad de Y fijarla en funcion a la Direccion del input solo si esta en una Plataforma.
        // De esta manera el Script de Platform puede detectar que la velocidad es negativa para poder funcionar
        int verticalDirection = controller.OnPlatform() && inputs.FixedAxis.y < 0 ? inputs.FixedAxis.y : 1;
        controller.SetVelocityY(stats.jumpVelocity * verticalDirection);
    }

    protected override void DoTransitionOut()
    {
        platformManager.LogicExit();
        base.DoTransitionOut();
    }

    protected override void TransitionChecks()
    {
        if (inputs.DashInput) 
        {       
            if(counter<= stats.dashCoyoteTime)
            {
                _target.ChangeState<PlayerDashJumpState>();
            }
            else
            {
                _target.ChangeState<PlayerDashState>();
            }
            inputs.UsedDash();          
        }
        else if(inputs.JumpCancel){
            controller.SetVelocityY(controller.CurrentVelocity.y * stats.shortHopMultiplier);
            _target.QueueAnimation(_target.animations.airPeak.name, true, true);
            stateDone = true;
        }   
        else if(controller.CurrentVelocity.y <=0 && !controller.OnPlatform()){
            _target.QueueAnimation(_target.animations.airPeak.name, true, true);
            stateDone = true;
        } 
        else if (controller.Grounded() && controller.CurrentVelocity.y<.1f)
        {
            _target.ChangeState<PlayerLandState>();
        }
        else if (inputs.HookInput)
        {
            _target.ChangeState<PlayerHookState>();
            inputs.UsedHook();
        }
        else if(inputs.MeleeInput){
            _target.ChangeState<PlayerMeleeState>();
            inputs.UsedMelee();
        }
        else if(inputs.GuardInput)
        {
            _target.ChangeState<PlayerHardenState>();
            inputs.UsedGuard();
        }
        else if(inputs.RangeInput){
            _target.ChangeState<PlayerRangeState>();
            inputs.UsedRange();
        }
        base.TransitionChecks();
    }
}
