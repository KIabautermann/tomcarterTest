using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : State<PlayerStateMachine>
{  
    protected PlayerInputHandler inputs;
    protected PlayerData stats;
    protected AbiltySystem abiltySystem;

    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        inputs = GetComponent<PlayerInputHandler>();
        abiltySystem = GetComponent<AbiltySystem>();
        stats = target.stats;
    }

}
