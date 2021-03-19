using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerGroundedState : PlayerState
{

    public PlayerGroundedState(PlayerController player, PlayerStateMachine stateMachine, PlayerData playerData, string currentAnimation) : base(player, stateMachine, playerData, currentAnimation)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        player.Jump.Reset();
        player.Dash.Reset();
        axis = player.MyInputs.FixedAxis;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        player.SetVelocityX(playerData.movementVelocity * player.facingDirection);
        if (!isExiting)
        {
            if (jumpInput && player.Jump.CanJump())
            {
                if (dashInput && player.Dash.CanDash())
                {
                    player.MyInputs.Jumped();
                    stateMachine.ChangeState(player.DashJump);
                }
                else
                {
                    player.MyInputs.Jumped();
                    stateMachine.ChangeState(player.Jump);
                }             
            }
            else if (!grounded)
            {
                player.InAir.JumpCoyoteTimeStart();
                stateMachine.ChangeState(player.InAir);
            }
            else if (dashInput && player.Dash.CanDash() && !player.Dash.Collision(player.MyInputs.FixedAxis))
            {
                if (jumpInput && player.Jump.CanJump())
                {
                    player.MyInputs.Jumped();
                    stateMachine.ChangeState(player.DashJump);
                }
                else
                {
                    stateMachine.ChangeState(player.Dash);
                }                
            }
            else if (hookInput && player.Hook.CanHook())
            {
                stateMachine.ChangeState(player.Hook);
            }
        }      
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

}
