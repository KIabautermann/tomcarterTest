using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAttackState : PlayerSkillState
{
    protected float startupTime;
    protected float hitboxTime;
    protected float recoveryTime;
    protected bool activeHitbox;
    protected Vector2 hitbox; 
    protected Vector3 hitboxOffset;
    
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
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        controller.LockFlip(true);
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
        controller.LockFlip(false);
        controller.FlipCheck(inputs.FixedAxis.x);
    }

    protected override void OnDestroyHandler()
    {
        base.OnDestroyHandler();
    }

    protected override void TransitionChecks()
    {
        base.TransitionChecks();
    }

    
}
