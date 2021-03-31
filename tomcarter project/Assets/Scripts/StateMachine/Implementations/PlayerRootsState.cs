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
            abilityDone = true;
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
        base.DoTransitionIn();
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
    }

    protected override void TransitionChecks()
    {
        if (_channelFinished) {
            // Add post rooting logic
            abilityDone = true;
        }

        base.TransitionChecks();
    }
}
