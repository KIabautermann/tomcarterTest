using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public abstract class PlayerDashState : PlayerUnlockableSkill
{
    protected Vector2 direction;
    protected bool coyoteTime;
    protected float coyoteStartTime;
    protected float currentSpeed;
    protected bool dashJumpCoyoteTime;
    private bool _velocityUpdated;

    private PlayerHedgeState playerHedgeState;
    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        coolDown = stats.dashCooldown;
        playerHedgeState = GetComponent<PlayerHedgeState>();
        playerHedgeState.onTransitionIn.AddListener(OnHedge_Handler);
        PlayerEventSystem.GetInstance().OnGroundLand += OnGround_Handler;
    }

    protected override void DoChecks()
    {
        base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
        DashJumpCoyoteTimeCheck();
        if(StartedDash()){
            controller.SetTotalVelocity(currentSpeed,direction);
            _velocityUpdated = true;
        } 
        if(!_velocityUpdated && Physics.Raycast(transform.position, direction, stats.collisionDetection, stats.hedge)) {
            controller.SetTotalVelocity(currentSpeed,direction);
        }  
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        inputs.UsedDash();
        controller.SetTotalVelocity(0,Vector2.zero);
        controller.SetAcceleration(1);
        controller.SetGravity(false);
        controller.SetDrag(stats.drag);
        coyoteTime = false;
        dashJumpCoyoteTime = controller.Grounded();
        Physics.IgnoreLayerCollision(9,10,true);
        _velocityUpdated = false;
    }

    protected bool StartedDash() => counter > + stats.dashStartUp;

    protected override void DoTransitionOut()
    {
        if (!controller.Grounded()) { isLocked = true; }

        Physics.IgnoreLayerCollision(9,10,false);
        base.DoTransitionOut();
        if(direction.x !=0)
        {
            if(inputs.FixedAxis.x == direction.x)
            {
                controller.SetAcceleration(1);
            }
            else
            {
                controller.SetAcceleration(.5f);
            }
        }
    }

    protected override void TransitionChecks()
    {
        base.TransitionChecks();
        Collider[] check = Physics.OverlapBox(transform.position, controller.myCollider.bounds.size/2, Quaternion.identity,stats.hedge);  
        if(check.Length != 0)
        {                  
            _target.ChangeState<PlayerHedgeState>();  
            controller.SetDrag(0);
        }
        else if(inputs.JumpInput && dashJumpCoyoteTime)
        {
            _target.ChangeState<PlayerDashJumpState>();
            inputs.UsedJump();            
        }    
    }
    public void DashJumpCoyoteTimeStart() => dashJumpCoyoteTime = true;

    private void DashJumpCoyoteTimeCheck()
    {
        if (dashJumpCoyoteTime && counter > stats.jumpHandicapTime)
        {
            dashJumpCoyoteTime = false;
        }
    }
   

    #region Event Handlers
    private void OnGround_Handler(object sender, PlayerEventSystem.OnLandEventArgs args) {
        ToggleLock(false);
    }
    private void OnHedge_Handler() {
        ToggleLock(false);
    }
    protected override void OnDestroyHandler() {
        PlayerEventSystem.GetInstance().OnGroundLand -= OnGround_Handler;
        playerHedgeState.onTransitionIn.RemoveListener(OnHedge_Handler);
        base.OnDestroyHandler();
    }

    #endregion
}
