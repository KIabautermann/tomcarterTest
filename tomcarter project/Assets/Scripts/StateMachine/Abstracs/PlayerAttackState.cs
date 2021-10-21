using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAttackState : PlayerSkillState
{
    protected bool activeHitbox;
    protected float attackDuration;
    protected bool onAir;
    protected bool hitDetection;
    protected bool forceAplied;
    public float time;

    protected override void DoChecks()
    {
        base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
        time = counter;
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        controller.LockFlip(true);
        activeHitbox = false;
        forceAplied = false;
        onAir = !controller.Grounded();
        hitDetection = false;
        
    }
    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
        controller.LockFlip(false);
    }
    protected override void OnDestroyHandler()
    {
        base.OnDestroyHandler();
    }
    protected override void TransitionChecks()
    {
        base.TransitionChecks();
        if (counter >= attackDuration)
        {
           stateDone = true;
        }
    } 

    public void SetHitboxOn()
    {
        activeHitbox = true;
    }
    
}
