using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashJumpState : PlayerSkillState
{
    public PlayerDashJumpState(PlayerController player, PlayerStateMachine stateMachine, PlayerData playerData, string currentAnimation) : base(player, stateMachine, playerData, currentAnimation)
    {
    }

    private bool _isJumping;

    public override void Enter()
    {
        base.Enter();
        player.RB.drag = 0;
        player.RB.useGravity = true;
        player.SetAcceleration(Mathf.Abs(axis.x));
        player.SetVelocityX(playerData.dashJumpVelocityX * player.facingDirection);
        player.SetVelocityY(playerData.jumpVelocity);
        player.SetParticles(true);
        _isJumping = true;
    }

    public override void Exit()
    {
        base.Exit();
        player.SetParticles(false);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        setJumpVelocity();
        player.FlipCheck(axis.x);
        if (axis.x == 0)
        {
            player.Accelerate(-1f / playerData.dashJumpAccelerationTime * Time.deltaTime);
        }
        else
        {
            player.Accelerate(1f / playerData.dashJumpAccelerationTime * Time.deltaTime);
        }

        player.SetVelocityX(playerData.dashJumpVelocityX * player.facingDirection);
        if (grounded && player.CurrentVelocity.y <= 0 && !abilityDone)
        {
            stateMachine.ChangeState(player.Land);        
        }
        else if (hookInput && player.Hook.CanHook())
        {
            stateMachine.ChangeState(player.Hook);
        }
        ClampYVelocity();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (player.CurrentVelocity.y < playerData.minDashJumpVelocity)
        {
            player.Force(Physics.gravity, playerData.dashJumpFallMultiplier);
        }
    }

    private void setJumpVelocity()
    {
        if (_isJumping)
        {
            if (jumpCancel)
            {
                player.SetVelocityY(player.CurrentVelocity.y * playerData.shortHopMultiplier);
                _isJumping = false;
            }
            else if (player.CurrentVelocity.y <= 0)
            {
                _isJumping = false;
            }
        }
    }
}
