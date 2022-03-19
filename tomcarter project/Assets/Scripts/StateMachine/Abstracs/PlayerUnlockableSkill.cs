using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class PlayerUnlockableSkill : PlayerSkillState
{
    protected int? _remainingUses;
    protected PlayerAbilitySystem abilitySystem;

    protected override void DoTransitionOut()
    {
        if (!abilitySystem.IsPermanentlyUnlocked(this.GetType())) 
        {
            _remainingUses--;
            if (_remainingUses == 0) 
            {
                abilitySystem.ExpireSkillStateUses(this.GetType()); 
                _remainingUses = abilitySystem.GetSkillUsesAmount(this.GetType());
            }
        }
        base.DoTransitionOut();
    }
    
    public override void Init(PlayerStateMachine target) {
        base.Init(target);
        abilitySystem = GetComponent<PlayerAbilitySystem>();
        _remainingUses = abilitySystem.GetSkillUsesAmount(this.GetType());
    }
}
