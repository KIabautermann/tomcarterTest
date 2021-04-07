﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerTransientState : PlayerState
{
    protected bool stateDone;

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
        stateDone = false;
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
    }

    protected override void TransitionChecks()
    {
        base.TransitionChecks();

        if(stateDone)
        {
            if (controller.Grounded())
            {
                _target.ChangeState<PlayerIdleState>();
            }
            else
            {
                airState.SetJump(false);
                _target.ChangeState<PlayerOnAirState>();
            }           
        }
    }
}