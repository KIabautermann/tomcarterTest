using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerSkillState
{
   private PlayerOnAirState onAir;

   private void Start() 
   {
       onAir = GetComponent<PlayerOnAirState>();      
   }
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
        base.DoTransitionIn();
        controller.SetVelocityY(stats.jumpVelocity);
        inputs.UsedJump();
        stateDone = true;
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
        onAir.SetJump(true);
    }

    protected override void TransitionChecks()
    {
        base.TransitionChecks();
    }
}
