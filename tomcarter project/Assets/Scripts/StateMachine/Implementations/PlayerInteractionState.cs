using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionState : PlayerState
{
    protected bool interactionFinished;
    protected Vector2 direction;
    protected override void DoChecks()
    {

    }
    protected override void DoLogicUpdate()
    {
       
    }

    protected override void DoPhysicsUpdate()
    {
        
    }

    protected override void DoTransitionIn()
    {
        interactionFinished = false;
        direction = new Vector2(controller.facingDirection,0);
        controller.SetVelocityX(0f);
        controller.SetAcceleration(0f);
    }

    protected override void DoTransitionOut()
    {
        interactionFinished = false;
    }

    protected override void TransitionChecks()
    {
        if (interactionFinished)
        {
            _target.ChangeState<PlayerIdleState>();
        }

        base.TransitionChecks();
    }
}
