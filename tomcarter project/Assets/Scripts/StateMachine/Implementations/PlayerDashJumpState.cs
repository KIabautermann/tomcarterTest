using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashJumpState : PlayerSkillState
{
    private bool _isJumping;
    
    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        animationTrigger = stats.dashJumpID;
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
        if (controller.CurrentVelocity.y <= stats.minJumpVelocity && !controller.Grounded() && controller.usingGravity)
        {
            controller.Force(Physics.gravity.normalized, stats.fallMultiplier, ForceMode.Force);
        }
        controller.SetVelocityX(stats.dashJumpVelocityX * controller.lastDirection);
        if (extraCounter >= stats.dashJumpAfterimageCounter)
        {
            _target.vfxSpawn.InstanceEffect(null, transform.position, Quaternion.identity, _target.vfxSpawn.EffectRepository.afterimage);
            extraCounter = 0;
        }
    }

    protected override void DoTransitionIn()
    {
        
        base.DoTransitionIn();
        _target.QueueAnimation(_target.animations.airUpwards.name, false, true);
        _isJumping = true;

        StartBlastEffect();

        controller.SetDrag(0);
        controller.SetGravity(true);
        controller.SetAcceleration(Mathf.Abs(inputs.FixedAxis.x));
        controller.SetVelocityX(stats.dashJumpVelocityX * controller.facingDirection);
        controller.SetVelocityY(stats.jumpVelocity);

        stateIndex = stats.airNumberID;
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
            GetComponent<PlayerRangeState>().ComingFromDashJump();
            _target.ChangeState<PlayerRangeState>();          
            inputs.UsedRange();
        }
        else if (controller.Grounded() && !stateDone && controller.CurrentVelocity.y < 0.1f)
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

    private void StartBlastEffect()
    {
        RaycastHit groundHit;
        if(!Physics.Raycast(gameObject.transform.position, Vector2.down, out groundHit, 1.5f, stats.walkable)){
            Debug.LogWarning("No hay lugar donde apoyar el blast zone del dash jump");
            return;
        }
     
        Vector3 pos = new Vector3(this.gameObject.transform.position.x - 0.5f * controller.facingDirection, groundHit.collider.bounds.max.y + 0.48f, 0f);
        Quaternion quaternion = controller.facingDirection != 1 ? Quaternion.Euler(0.0f, 180.0f, 0.0f) : Quaternion.identity;
        _target.vfxSpawn.InstanceEffect(null, pos, quaternion, _target.vfxSpawn.EffectRepository.airBlast);
    }
    
    public void SetAnimationIndex(int newIndex) 
    {
        animationIndex = newIndex;
    }
}
