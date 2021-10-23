using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBounceJumpState : PlayerBasicMovementState
{
    private bool _fromDashJump;
    public override void Init(PlayerStateMachine target)
    {
        animationTrigger = "bounce";
        base.Init(target);
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
        controller.FlipCheck(inputs.FixedAxis.x);
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        canShortHop = false;
        controller.SetVelocityY(stats.jumpVelocity);
        _target.QueueAnimation(_target.animations.airUpwards.name, false, false);
        currentAcceleration = _fromDashJump ? stats.airAccelerationTime : stats.dashJumpAccelerationTime;
        currentSpeed = _fromDashJump ? stats.dashJumpVelocityX : stats.movementVelocity;
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
        _fromDashJump = false;
    }

    protected override void TransitionChecks()
    {
        base.TransitionChecks();
        Vector3 direction = controller.CurrentVelocity.normalized;
        base.TransitionChecks();
        if (Physics.Raycast(_target.transform.position, direction, stats.collisionDetection, stats.hedge))
        {
            _target.ChangeState<PlayerHedgeState>();
        }
        else if (inputs.HookInput)
        {
            _target.ChangeState<PlayerHookState>();
            inputs.UsedHook();
        }
        else if (inputs.DashInput)
        {
            _target.ChangeState<PlayerDashState>();
            inputs.UsedHook();
        }
        else if (inputs.RangeInput)
        {
            if(_fromDashJump)GetComponent<PlayerRangeState>().ComingFromDashJump();
            _target.ChangeState<PlayerRangeState>();
            inputs.UsedRange();
        }
        else if (controller.Grounded() && !stateDone)
        {
            _target.ChangeState<PlayerLandState>();
        }

    }

    public void CommingFromDashJump(bool dashjump)
    {
        _fromDashJump = dashjump;
    }
}
