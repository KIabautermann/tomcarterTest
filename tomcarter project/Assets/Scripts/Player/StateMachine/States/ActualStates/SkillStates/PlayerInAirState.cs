using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    private bool _isJumping;
    private bool _jumpCoyoteTime;
    private bool _dashJumpCoyoteTime;

    public PlayerInAirState(PlayerController player, PlayerStateMachine stateMachine, PlayerData playerData, string currentAnimation) : base(player, stateMachine, playerData, currentAnimation)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        JumpCoyoteTimeStart();
        if (_isJumping)
        {
            DashJumpCoyoteTimeStart();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        JumpCoyoteTimeCheck();
        DashJumpCoyoteTimeCheck();

        setJumpVelocity();

        if (grounded && player.CurrentVelocity.y < .01f)
        {
            stateMachine.ChangeState(player.Land);
        }
        else if (jumpInput && player.Jump.CanJump())
        {
            stateMachine.ChangeState(player.Jump);
        }
        else if (dashInput && player.Dash.CanDash() &&!player.Dash.Collision(player.MyInputs.FixedAxis))
        {
            if(_dashJumpCoyoteTime)
            {
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
        else
        {
            player.FlipCheck(axis.x);
            player.Accelerate((axis.x != 0 ? 1 / playerData.airAccelerationTime : -1 / playerData.airAccelerationTime) * Time.deltaTime);
            player.SetVelocityX(playerData.movementVelocity * player.facingDirection);
            if (player.OnWall() && axis.x != 0) 
            {
                player.SetAcceleration(.5f);
            }
        }
        ClampYVelocity();
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if(player.CurrentVelocity.y < playerData.minJumpVelocity)
        {
            player.Force(Physics.gravity, playerData.fallMultiplier);
        }
    }
    private void JumpCoyoteTimeCheck()
    {
        if(_jumpCoyoteTime && Time.time > startTime + playerData.coyoteTime)
        {
            _jumpCoyoteTime = false;
            player.Jump.Decrease();
        }
    }
    private void DashJumpCoyoteTimeCheck()
    {
        if (_dashJumpCoyoteTime && Time.time > startTime + playerData.jumpHandicapTime)
        {
            _dashJumpCoyoteTime = false;
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
            else if(player.CurrentVelocity.y <= 0)
            {
                _isJumping = false;
            }
        }
    }
    
    public void JumpCoyoteTimeStart() => _jumpCoyoteTime = true;

    public void DashJumpCoyoteTimeStart() => _dashJumpCoyoteTime = true;

    public void SetJump() => _isJumping = true;
}
