﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : State<PlayerStateMachine>
{  
    protected PlayerInputHandler inputs;
    protected PlayerData stats;

    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        inputs = GetComponent<PlayerInputHandler>();
        stats = target.stats;
    }

    protected override void DoTransitionIn()
    {
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
