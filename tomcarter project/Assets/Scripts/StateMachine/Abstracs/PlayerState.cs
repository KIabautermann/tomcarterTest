using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : State<PlayerStateMachine>
{  
    protected PlayerInputHandler inputs;
    protected PlayerData stats;
    protected MovementController controller;

    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        inputs = GetComponent<PlayerInputHandler>();
        controller = GetComponent<MovementController>();
        stats = target.stats;
    }

    protected override void DoTransitionIn()
    {
        // Idle esta pasando a idle, disparando multiples veces estos eventos. No se si esta contemplado
        // Cada estado que llame a este base, esta condenado a terminar en idle?
        StartCoroutine(ToIdle());
    }

    protected override void DoTransitionOut()
    {
        StopAllCoroutines();
    }

    public IEnumerator ToIdle()
    {
        yield return new WaitForSeconds(1);
        _target.ChangeState<PlayerIdleState>();
    }
}
