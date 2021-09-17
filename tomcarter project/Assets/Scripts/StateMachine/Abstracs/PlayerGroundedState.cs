using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerGroundedState : PlayerState
{
    private Vector2 direction;
    protected override void DoChecks()
    {   
        direction = new Vector2(controller.facingDirection,0);
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
        controller.SetVelocityX(stats.movementVelocity * controller.lastDirection);
    }

    protected override void DoPhysicsUpdate()
    {
        
    }

    protected override void DoTransitionIn()
    {
        direction = new Vector2(controller.facingDirection,0);
        base.DoTransitionIn();        
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
    }

    protected override void TransitionChecks()
    {
        if(controller.OnRootable() && inputs.RootsInput){
            _target.ChangeState<PlayerRootsState>();
            inputs.UsedRoots();
        }
        else if (inputs.InteractionInput && !inputs.InteractionCancel) {
            if(Physics.Raycast(_target.transform.position, direction,stats.npcDetectionLenght, stats.npc))
            {
                _target.ChangeState<PlayerDialogueState>();
            }
            inputs.UsedInteraction();
        }
        else if(inputs.JumpInput){
            _target.ChangeState<PlayerJumpState>();
            inputs.UsedJump();     
        }
        else if(inputs.DashInput){
            _target.ChangeState<PlayerDashState>();
            inputs.UsedDash();     
        }
        else if(inputs.MeleeInput){
            _target.ChangeState<PlayerMeleeState>();
            inputs.UsedMelee();
        }
        else if(inputs.RangeInput){
            _target.ChangeState<PlayerRangeChargeState>();
            inputs.UsedRange();
        }
        else if(inputs.HookInput){
            _target.ChangeState<PlayerHookState>();
            inputs.UsedHook();
        }
        else if(inputs.GuardInput)
        {
            _target.ChangeState<PlayerHardenState>();
            inputs.UsedGuard();
        }
        else if(!controller.Grounded())
        {
            _target.ChangeState<PlayerOnAirState>();
            GetComponent<PlayerOnAirState>().JumpCoyoteTimeStart();
        }

        base.TransitionChecks();
    }
}
