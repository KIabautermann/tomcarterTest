using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRangeChargeState : PlayerState
{
   
    private bool charged;
    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        asynchronous = true;
        animationTrigger = stats.chargeID;
        coolDown = stats.rangeCooldown;
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        charged = false;
        
    }
    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
        if(counter >= stats.rangeStartupTime + stats.rangeChargeTime && !charged){
            charged = true;
        }
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
    }

    protected override void TransitionChecks()
    {
        base.TransitionChecks();   
        if(inputs.RangeCancel){
            if(counter > stats.rangeStartupTime && !charged){
                _target.ChangeState<PlayerRangeState>();
                _target.removeSubState();
            }
            else if(charged){
                _target.ChangeState<PlayerChargedRangeState>();
                _target.removeSubState();
            }
        }
          
    }
}
