using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerSkillState : PlayerState
{
    protected bool abilityDone;
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
        base.DoTransitionIn();
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
    }

    protected override void TransitionChecks()
    {
        
    }
}
