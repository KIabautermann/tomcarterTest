using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class PlayerState : State<PlayerStateMachine>
{  
    protected PlayerInputHandler inputs;
    protected PlayerData stats;
    protected MovementController controller;
    protected PlayerHealth playerHealth;
    protected bool grounded;
    protected static bool tookDamage;

    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        inputs = GetComponent<PlayerInputHandler>();
        controller = GetComponent<MovementController>();
        playerHealth = GetComponent<PlayerHealth>();
        stats = target.stats;
        PlayerHealthEventSystem.GetInstance().OnDamageTaken += OnPlayerTakenDamageHandler;
    }

    protected override void DoChecks()
    {
        grounded = controller.Grounded();
    }

    protected override void DoTransitionIn()
    {
        
    }

    protected override void DoPhysicsUpdate()
    {
       
    }

    protected override void DoTransitionOut()
    {
        StopAllCoroutines();
    } 

    protected override void TransitionChecks()
    {
        // No me gusta tener esto en una variable estatica, pero no se me ocurrio una alternativa al caso donde el evento corra el handler
        // mientras un ChangeState esta en proceso, y por lo tanto la StateMachine ignora el proces. Si es una variable estatica, entonces todo
        // state corriendo va a poder ver que tomo daño en algun frame cercano.
        // Alternativas: 
        //      - Cambiar la StateMachine para que pueda encolar estados y forzar su ejecucion apenas el actual termine
        if (tookDamage)
        {
            tookDamage = false;
            _target.ChangeState<PlayerDamagedState>();
        }
    }

    private void OnPlayerTakenDamageHandler(object sender, EventArgs e)
    {
        // Tecnicamente no pasa nada si todos los estados corren el handler, pero mierda que esto es una solucion polemica.
        // Hay alguna manera de que la clase abstracta tenga este codigo, pero no todos corran su handlers?
        if (_target.IsCurrentState(this))
        {
            tookDamage = true;
        }
    }

    protected void ClampYVelocity()
    {
        if (controller.CurrentVelocity.y <= stats.maxFallVelocity)
        {
            controller.SetVelocityY(stats.maxFallVelocity);
        }
    }
}
