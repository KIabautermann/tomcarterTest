using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerTransientState : PlayerState
{

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
        
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        stateDone = false;
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
    }

    protected override void TransitionChecks()
    {
        base.TransitionChecks();
    }


}
