using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : State<PlayerStateMachine>
{  
    protected PlayerInputHandler inputs;
    protected PlayerData stats;
    protected MovementController controller;
    protected bool grounded;

    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        inputs = GetComponent<PlayerInputHandler>();
        controller = GetComponent<MovementController>();
        stats = target.stats;
    }

    protected override void DoChecks()
    {
        grounded = controller.Grounded();
    }

    protected override void DoTransitionIn()
    {
        
    }

    protected override void DoPhysicsUpdate()
    {
       
    }

    protected override void DoTransitionOut()
    {
        StopAllCoroutines();
    }

    protected void ClampYVelocity()
    {
        if (controller.CurrentVelocity.y <= stats.maxFallVelocity)
        {
            controller.SetVelocityY(stats.maxFallVelocity);
        }
    }
}
