using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerSkillState : PlayerState
{
    protected bool abilityDone;

    public PlayerSkillState(PlayerController player, PlayerStateMachine stateMachine, PlayerData playerData, string currentAnimation) : base(player, stateMachine, playerData, currentAnimation)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        abilityDone = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (abilityDone)
        {
            if (grounded && player.CurrentVelocity.y < .01f)
            {
                stateMachine.ChangeState(player.Idle);
            }
            else
            {
                player.Jump.Decrease();
                stateMachine.ChangeState(player.InAir);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
