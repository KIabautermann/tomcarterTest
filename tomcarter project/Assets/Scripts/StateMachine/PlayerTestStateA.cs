using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTestStateA : PlayerState
{
  
    protected override void DoChecks()
    {
        
    }

    protected override void DoLogicUpdate()
    {
        TransitionChecks();
    }

    protected override void DoPhysicsUpdate()
    {
       
    }

    protected override void DoTransitionIn()
    {
        Debug.Log("State A");
    }

    protected override void DoTransitionOut()
    {
        
    }

    protected override void TransitionChecks()
    {
        if(inputs.JumpInput)
        {
            _target.ChangeState<PlayerTestStateB>();
            inputs.UsedJump();
        }
    }
}
