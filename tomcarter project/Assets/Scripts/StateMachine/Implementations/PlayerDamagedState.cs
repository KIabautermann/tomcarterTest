using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamagedState : PlayerState
{
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
    
    }

    protected override void DoTransitionOut()
    {
        
    }

    protected override void TransitionChecks()
    {
        if (Time.time > startTime + playerHealth.invulnerabilityPeriod) 
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
