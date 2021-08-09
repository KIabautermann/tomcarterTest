﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashJumpState : PlayerSkillState
{
    private bool _isJumping;
    private ObjectPooler afterImagePooler;
    private GameObject afterImageParent;
    
    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        animationTrigger = stats.dashJumpID;
        stateIndex = stats.airNumberID;
        afterImagePooler = target.afterImagePooler;
        afterImageParent = new GameObject("DashJumpAfterImages");
    }
    protected override void DoChecks()
    {
        base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
        platformManager.LogicUpdated();
        setJumpVelocity();
        controller.FlipCheck(inputs.FixedAxis.x);
        if (inputs.FixedAxis.x == 0)
        {
            controller.Accelerate(-1f / stats.dashJumpAccelerationTime * Time.deltaTime);
        }
        else
        {
            controller.Accelerate(1f / stats.dashJumpAccelerationTime * Time.deltaTime);
        }
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
        if (controller.CurrentVelocity.y < stats.minDashJumpVelocity)
        {
            if (animationIndex < 3) animationIndex = 2;
            controller.Force(Physics.gravity.normalized, stats.dashJumpFallMultiplier, ForceMode.Force);
        }
        controller.SetVelocityX(stats.dashJumpVelocityX * controller.lastDirection);
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        _isJumping = true;
        controller.SetDrag(0);
        controller.SetGravity(true);
        controller.SetAcceleration(Mathf.Abs(inputs.FixedAxis.x));
        controller.SetVelocityX(stats.dashJumpVelocityX * controller.facingDirection);
        controller.SetVelocityY(stats.jumpVelocity);

        animationTrigger = stats.airID;
        stateIndex = stats.airNumberID;
        
        StartCoroutine(AfterImageCoroutine());
    }

    protected override void DoTransitionOut()
    {
        platformManager.LogicExit();
        base.DoTransitionOut();
    }

    protected override void TransitionChecks()
    {
        Vector3 direction = controller.CurrentVelocity.normalized;
        base.TransitionChecks();
        if(Physics.Raycast(_target.transform.position, direction,stats.collisionDetection, stats.hedge))
        {
            _target.ChangeState<PlayerHedgeState>();             
        }
        else if (inputs.HookInput)
        {
            _target.ChangeState<PlayerHookState>();
            inputs.UsedHook();
        }
        else if(inputs.RangeInput){
            _target.ChangeState<PlayerRangeState>();
            inputs.UsedRange();
        }
        else if (controller.Grounded() && !stateDone)
        {
            _target.ChangeState<PlayerLandState>();        
        }      
    }

    private void setJumpVelocity()
    {
        if (_isJumping)
        {
            if (inputs.JumpCancel)
            {
                controller.SetVelocityY(controller.CurrentVelocity.y * stats.shortHopMultiplier);
                _isJumping = false;
            }
            else if (controller.CurrentVelocity.y <= 0)
            {
                _isJumping = false;
            }
        }
    }
    private IEnumerator AfterImageCoroutine()
    {
        while (true) {
            
            yield return new WaitForSeconds(0.042f);

            ComponentCache<MonoBehaviour> afterImageComponents = afterImagePooler.GetItem(Vector3.zero, Quaternion.identity);
            afterImageComponents.GetInstance(typeof(PlayerAfterImageSprite), out MonoBehaviour tmp);
            PlayerAfterImageSprite pais = tmp as PlayerAfterImageSprite;

            pais.gameObject.transform.SetParent(afterImageParent.transform, true);
            
            if (controller.facingDirection != 1) pais.gameObject.transform.Rotate(0.0f, 180.0f, 0.0f);
            
            pais.LogicStart(this.gameObject.transform.position, stateIndex, animationIndex, Mathf.RoundToInt(counter - stats.dashStartUp));
        }
    }
    
    public void SetAnimationIndex(int newIndex) 
    {
        animationIndex = newIndex;
    }
}
