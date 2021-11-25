using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeState : PlayerAttackState
{
    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        attackDuration = stats.meleeTime;
        animationTrigger = stats.meleeID;
    }

    protected override void DoChecks()
    {
        base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
        if (onAir)
        {
            canMove = true;
            currentAcceleration = stats.airAccelerationTime;
        }
        else
        {
            canMove = false;
            currentAcceleration = stats.groundedAccelerationTime;
        }
        controller.FlipCheck(inputs.FixedAxis.x);
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        if (!onAir)
        {
            _target.QueueAnimation(_target.animations.attackGround.name, false, true);
        }
        else
        {
            _target.QueueAnimation(_target.animations.attackGround.name, false, true);
        }
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
        controller.LockFlip(false);
    }

    protected override void TransitionChecks()
    {
        base.TransitionChecks();
        if(onAir && controller.Grounded())
        {
            _target.ChangeState<PlayerLandState>();
        }
    }
}
