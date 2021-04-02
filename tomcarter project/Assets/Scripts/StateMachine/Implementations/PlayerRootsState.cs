using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRootsState : PlayerSkillState
{
    private bool _channelFinished = false;
    protected override void DoChecks()
    {
         base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        if (inputs.RootsCancel) 
        {
            stateDone = true;
        }
        else if (Time.time > startTime + stats.rootChannelingDuration)
        {
            _channelFinished = true;
        }

        base.DoLogicUpdate();
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
    }

    protected override void DoTransitionIn()
    {
        _channelFinished = false;
        controller.SetAcceleration(0f);
        controller.SetVelocityX(0f);
        base.DoTransitionIn();
    }

    protected override void DoTransitionOut()
    {
        // Add post rooting logic
        base.DoTransitionOut();
    }

    protected override void TransitionChecks()
    {
        if (_channelFinished) {
            stateDone = true;
        }

        base.TransitionChecks();
    }
}
