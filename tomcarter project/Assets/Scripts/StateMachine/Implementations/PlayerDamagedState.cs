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
        _target.removeSubState();  
    }

    protected override void DoTransitionOut()
    {
        
    }

    protected override void TransitionChecks()
    {
        // TODO: Hay un bug que si dasheas en el aire antes de entrar a este estado, el movimiento del Bushy se vuelve mas lento que Maradona intentando leer en ingles
        if (counter > + playerHealth._invulnerabilityPeriod) 
        {
            stateDone = true;

            base.TransitionChecks();
        }
    }

}
