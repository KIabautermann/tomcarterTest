using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class PlayerState : State<PlayerStateMachine>
{  
    protected PlayerInputHandler inputs;
    protected PlayerData stats;
    protected MovementController controller;
    protected Platform platformManager;
    protected PlayerHealth playerHealth;
    protected static bool tookDamage;
    protected bool stateDone;

    protected PlayerOnAirState airState;
    

    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        inputs = GetComponent<PlayerInputHandler>();
        controller = GetComponent<MovementController>();
        playerHealth = GetComponent<PlayerHealth>();
        stats = target.stats;
        airState = GetComponent<PlayerOnAirState>();
        platformManager = GetComponent<Platform>();
    }

    protected override void DoChecks()
    {
        
    }

    protected override void DoLogicUpdate()
    {
        controller.ClampYVelocity(stats.maxFallVelocity);
    }

    protected override void DoTransitionIn()
    {
        counter = 0;
        PlayerEventSystem.GetInstance().OnDamageTaken += OnPlayerTakenDamageHandler;
        stateDone = false;
    }

    protected override void DoPhysicsUpdate()
    {
      
    }

    protected override void DoTransitionOut()
    {
        PlayerEventSystem.GetInstance().OnDamageTaken -= OnPlayerTakenDamageHandler;
        StopAllCoroutines();
    } 

    // Tenemos que considerar posibles casos donde un base state hace una transicion, la state machine ejecuta el codigo de las TransitionIn/Out, pero despues
    // un override de este metodo tambien llama a ChangeState. En este caso se podrian disparar animaciones a un estado al que no se transiciona
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
        if (stateDone)
        {
            if (controller.Grounded())
            {
                _target.ChangeState<PlayerIdleState>();
            }
            else
            {
                _target.ChangeState<PlayerOnAirState>();
            }
        }
    }

    private void OnPlayerTakenDamageHandler(object sender, EventArgs e)
    {
        tookDamage = true;
    }

    protected override void OnDestroyHandler() {}
}