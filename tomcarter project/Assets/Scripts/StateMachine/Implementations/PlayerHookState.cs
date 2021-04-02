using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHookState : PlayerSkillState
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
        base.DoPhysicsUpdate();
    }

    protected override void DoTransitionIn()
    {
        // Place Holder
        controller.SetVelocityX(0f);
        controller.SetAcceleration(0f);
        StartCoroutine(WaitAndEndAbility());
        // End Place Holder
        base.DoTransitionIn();
    }

    private IEnumerator WaitAndEndAbility()
    { 
        yield return new WaitForSeconds(1.5f); 
        stateDone = true;
    }
    
    protected override void DoTransitionOut()
    {
        StopAllCoroutines();
        base.DoTransitionOut();
    }

    protected override void TransitionChecks()
    {
        if(inputs.GuardInput){
            _target.ChangeState<PlayerGuardState>();
            inputs.UsedGuard();
        }
        else
        {
            base.TransitionChecks();
        }
       
    }
}
