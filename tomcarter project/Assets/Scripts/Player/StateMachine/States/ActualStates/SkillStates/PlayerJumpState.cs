using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerSkillState
{
    private int jumpsLeft;
    public PlayerJumpState(PlayerController player, PlayerStateMachine stateMachine, PlayerData playerData, string currentAnimation) : base(player, stateMachine, playerData, currentAnimation)
    {
        jumpsLeft = playerData.amountOfJumps;
    }

    public override void Enter()
    {
        base.Enter();
        Decrease();
        if(player.MyInputs.dashInput && player.Dash.CanDash())
        {
            stateMachine.ChangeState(player.DashJump);
        }
        else
        {
            player.SetVelocityY(playerData.jumpVelocity);
            player.InAir.SetJump();
            abilityDone = true;
        }            
    }

    public bool CanJump() => jumpsLeft > 0;

    public void Reset() => jumpsLeft = playerData.amountOfJumps;

    public void Decrease() => jumpsLeft--;
}
