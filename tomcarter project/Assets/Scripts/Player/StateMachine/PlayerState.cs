using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    protected PlayerController player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;
    protected float startTime;
    protected bool grounded;
    protected bool onWall;
    protected bool animationFinished;
    protected bool isExiting;
    protected Vector2Int axis;
    protected bool jumpInput;
    protected bool jumpCancel;
    protected bool dashInput;
    protected bool hookInput;

    public string currentAnimation { get; private set; }

    public PlayerState(PlayerController player, PlayerStateMachine stateMachine, PlayerData playerData, string currentAnimation)
    {
        this.player = player; 
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.currentAnimation = currentAnimation;
    }

    public virtual void Enter()
    {
        DoChecks();
        axis = player.MyInputs.FixedAxis;
        isExiting = false;
        player.Anim.SetBool(currentAnimation, true);
        startTime = Time.time;
        animationFinished = false;
    }
    public virtual void Exit()
    {
        player.Anim.SetBool(currentAnimation, false);
        isExiting = true;
    }
    public virtual void LogicUpdate()
    {
        axis = player.MyInputs.FixedAxis;
        jumpInput = player.MyInputs.jumpInput;
        jumpCancel = player.MyInputs.jumpStop;
        dashInput = player.MyInputs.dashInput;
        hookInput = player.MyInputs.hookInput;
    }
    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    public virtual void DoChecks()
    {
        grounded = player.Grounded();
        onWall = player.OnWall();
    }

    public virtual void FrameEndUpdate()
    {
        
    }

    protected void ClampYVelocity()
    {
        if (player.CurrentVelocity.y <= playerData.maxFallVelocity)
        {
            player.SetVelocityY(playerData.maxFallVelocity);
        }
    }

    public virtual void AnimationTrigger() { }
    

    public virtual void AnimationFinishedTrigger() => animationFinished = true;
    
}
