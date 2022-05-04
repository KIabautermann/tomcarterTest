using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerDamagedState : PlayerTransientState
{
    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        animationTrigger = stats.damageTrigger;
    }
    protected override void DoChecks()
    {
        base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
       
    }

    protected override void DoPhysicsUpdate()
    {
        
    }

    protected override void DoTransitionIn()
    {
        controller.SetTotalVelocity(0f, Vector2.right);
        controller.SetAcceleration(0f);

        base.DoTransitionIn();
    }

    protected override void DoTransitionOut()
    {
        controller.SetTotalVelocity(0f, Vector2.right);
        controller.SetAcceleration(0f);
        base.DoTransitionOut();
    }

    protected override void TransitionChecks()
    {
        if (stateDone && controller.Grounded()) {
            _target.ChangeState<PlayerIdleState>();
        }
        
        playerHealth.currentHealth--;

        Collider[] _hazardHit = Physics.OverlapBox(
            transform.position, 
            controller.myCollider.bounds.size/2 * 1.1f,
            Quaternion.identity, stats.hazard);

        if (stateDone) return;

        stateDone = true;

        if (_hazardHit.Length > 0) {
            PlayerEventSystem.GetInstance().TriggerPlayerCollidedHazard();
        } else {
            base.TransitionChecks();
        }
    }

}
