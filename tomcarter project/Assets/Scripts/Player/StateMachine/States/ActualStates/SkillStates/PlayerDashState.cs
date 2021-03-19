using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerSkillState
{
    public bool dashAvaliable { get; private set; }
    private float lastDashTime;
    private bool isHolding;
    private Vector2 directionInput;
    private Vector2 direction;
    private bool coyoteTime;
    private bool wastedCoyoteTime;
    private float coyoteStartTime;
    public PlayerDashState(PlayerController player, PlayerStateMachine stateMachine, PlayerData playerData, string currentAnimation) : base(player, stateMachine, playerData, currentAnimation)
    {
    }

    public override void Enter()
    {
        base.Enter();
        dashAvaliable = false;
        player.MyInputs.Dashed();
        isHolding = true;
        direction = Vector2.right * player.facingDirection;
        coyoteTime = false;
        wastedCoyoteTime = !player.Grounded();
    }

    public override void Exit()
    {
        base.Exit();       
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!isExiting)
        {
            if (!wastedCoyoteTime)
            {
                CoyoteTimeCheck();
            }         
            if (isHolding)
            {
                directionInput = axis;
                if (directionInput != Vector2.zero)
                {
                    direction = directionInput;
                    direction.Normalize();
                }
                if (Time.time >= startTime + playerData.HoldTime)
                {
                    isHolding = false;
                    player.SetParticles(true);
                    startTime = Time.time;
                    player.FlipCheck(Mathf.RoundToInt(direction.x));
                    player.RB.drag = playerData.drag;
                    player.RB.useGravity = false;
                    player.SetAcceleration(0);
                    player.SetTotalVelocity(playerData.dashSpeed, direction.normalized);
                }
            }
            else
            {
                if (Time.time > startTime + playerData.dashLenght)
                {
                    player.RB.drag = 0;
                    player.RB.useGravity = true;
                    player.SetParticles(false);
                    abilityDone = true;
                    lastDashTime = Time.time;
                    if (player.CurrentVelocity.y > 0)
                    {
                        player.SetVelocityY(player.CurrentVelocity.y * playerData.dashEndMultiplier);
                    }
                    if (player.CurrentVelocity.x != 0)
                    {
                        player.SetAcceleration(1);
                    }
                }
            }
            if(jumpInput && player.CurrentVelocity.y <= 0)
            {
                if(coyoteTime || player.Grounded())
                {
                    isExiting = true;
                    player.MyInputs.Jumped();
                    player.StateMachine.ChangeState(player.DashJump);
                    player.SetAcceleration(0);
                }               
            }
            else if(Physics.Raycast(player.transform.position, direction,playerData.hedgeDetectionLenght, playerData.hedge))
            {
                stateMachine.ChangeState(player.Hedge);
                player.RB.drag = 0;
                player.SetParticles(false);
            }
        }       
    }

    public bool CanDash() => dashAvaliable && Time.time >= lastDashTime + playerData.dashCooldown;

    public bool Collision(Vector2 currentDirection)
    {
        if (currentDirection.magnitude != 0)
        {
            return Physics.Raycast(player.RB.position, currentDirection, playerData.collisionDetection, playerData.walkable);
        }
        return Physics.Raycast(player.RB.position, player.transform.right, playerData.collisionDetection, playerData.walkable);
    }

    public void Reset() => dashAvaliable = true;

    public void DashTime()
    {
        lastDashTime = Time.time;
    }

    private void CoyoteTimeCheck()
    {
        if (!player.Grounded() && !coyoteTime)
        {
            coyoteTime = true;
            coyoteStartTime = Time.time;
        }
        else if(!player.Grounded() && Time.time>coyoteStartTime + playerData.dashCoyoteTime)
        {
            coyoteTime = false;
            wastedCoyoteTime = true;
        }
    }

   
}
