using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerGroundedState
{
    public PlayerLandState(PlayerController player, PlayerStateMachine stateMachine, PlayerData playerData, string currentAnimation) : base(player, stateMachine, playerData, currentAnimation)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (axis.x != 0)
        {
            stateMachine.ChangeState(player.Move);
        }
        else
        {
            stateMachine.ChangeState(player.Idle);
        }
    }
}
