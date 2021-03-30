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
        abilityDone = false;
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
    }

    protected override void TransitionChecks()
    {
        if(abilityDone)
        {
            if(!grounded)
            {
                _target.ChangeState<PlayerOnAirState>();
            }
            else
            {
                if(inputs.FixedAxis.x != 0)
                {
                    _target.ChangeState<PlayerMovementState>();
                }
                else
                {
                    _target.ChangeState<PlayerIdleState>(); 
                }
            }
        }
    }
}
