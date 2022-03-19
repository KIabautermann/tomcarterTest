using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamagedState : PlayerTransientState
{
    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        animationTrigger = stats.damageTrigger;
    }
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
        controller.SetTotalVelocity(0f, Vector2.right);
        controller.SetAcceleration(0f);
        base.DoTransitionIn();
    }

    protected override void DoTransitionOut()
    {
        controller.SetTotalVelocity(0f, Vector2.right);
        controller.SetAcceleration(0f);
    }

    protected override void TransitionChecks()
    {
        // TODO: Hay un bug que si dasheas en el aire antes de entrar a este estado, el movimiento del Bushy se vuelve mas lento que Maradona intentando leer en ingles
        if (counter > + playerHealth._invulnerabilityPeriod) 
        {
            stateDone = true;
        }
        
        base.TransitionChecks();
    }

}
