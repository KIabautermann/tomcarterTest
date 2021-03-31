using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerSkillState : PlayerState
{
    protected bool abilityDone;
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
        base.DoTransitionIn();
        abilityDone = false;
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
    }

    protected override void TransitionChecks()
    {
        base.TransitionChecks();
        
        if(abilityDone)
        {
            Debug.Log(controller.CurrentVelocity.y);
            if (grounded)
            {
                _target.ChangeState<PlayerIdleState>();
            }
            else
            {
                _target.ChangeState<PlayerOnAirState>();
            }           
        }
    }
}
