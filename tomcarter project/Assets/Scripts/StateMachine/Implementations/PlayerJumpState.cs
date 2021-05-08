using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerSkillState
{ 
    private bool _dashJumpCoyoteTime;
    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        animationTrigger = stats.jumpTrigger;
    }
   
   protected override void DoChecks()
    {
        base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
        
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
        if(inputs.JumpCancel || counter >= stats.jumpLenght){
            controller.SetVelocityY(controller.CurrentVelocity.y * stats.shortHopMultiplier);
            stateDone = true;
        }
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        controller.SetVelocityY(stats.jumpVelocity);
        controller.SetGravity(false);
        inputs.UsedJump();
        _dashJumpCoyoteTime = true;
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
        controller.SetGravity(true);
    }

    protected override void TransitionChecks()
    {
        base.TransitionChecks();
        if(_dashJumpCoyoteTime && inputs.DashInput)
        {
            _target.ChangeState<PlayerDashJumpState>();
        }
    }

    private void DashJumpCoyoteTimeCheck()
    {
        if (_dashJumpCoyoteTime && counter > stats.jumpHandicapTime)
        {
            _dashJumpCoyoteTime = false;
        }
    }
}
