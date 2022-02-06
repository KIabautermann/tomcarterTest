using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRangeState : PlayerTransientState
{
    public Vector2 direction;
    private float lerpIndex;
    private bool casted;
    private float movementValue;
    private float currentSpeed;
    private bool dashJump;
    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        animationTrigger = stats.rangeID;
        coolDown = stats.rangeCooldown;
    }

    protected override void DoChecks()
    {
        base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
        controller.FlipCheck(inputs.FixedAxis.x);
        SetMovement();
        if (counter > stats.rangeCastTime & !casted)
        {
            Cast();
        }
        if (casted)
        {
            if (direction.x != 0)
            {
                lerpIndex += Time.deltaTime;
                movementValue = Mathf.Lerp(-direction.x * stats.rangeRecoil, currentSpeed * controller.lastDirection, lerpIndex);
            }
            else
            {
                if (!controller.Grounded())
                {
                    if (!dashJump) controller.Accelerate((inputs.FixedAxis.x != 0 ? 1 : -1) / stats.groundedAccelerationTime * Time.deltaTime);
                }
                if (dashJump)
                {
                    movementValue = currentSpeed * controller.facingDirection;
                    controller.SetAcceleration(1);
                    controller.Accelerate((inputs.FixedAxis.x != 0 ? 1 : -1 / stats.dashJumpAccelerationTime) * Time.deltaTime);
                }
            }
        }
        else
        {
            movementValue = currentSpeed * controller.lastDirection;
        }
        if (counter > stats.rangeCastTime + stats.rangeRecoveryTime)
        {
            if (!dashJump)
            {
                stateDone = true;
            }
            else if (controller.Grounded())
            {
                stateDone = true;
            }
        }
        if (controller.Grounded())
        {
            controller.Accelerate((-1 / stats.groundedAccelerationTime) * Time.deltaTime);
        }
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
        controller.SetVelocityX(movementValue);
        if (controller.CurrentVelocity.y <= stats.minJumpVelocity && !controller.Grounded() && controller.usingGravity && !_target.gravityException)
        {
            controller.Force(Physics.gravity.normalized, stats.fallMultiplier, ForceMode.Force);
        }
        else if (_target.gravityException)
        {
            controller.Force(-Physics.gravity.normalized, Physics.gravity.magnitude / 2, ForceMode.Force);
        }
        if (dashJump)
        {
            if (extraCounter >= stats.dashJumpAfterimageCounter)
            {
                _target.vfxSpawn.InstanceEffect(null, transform.position, transform.rotation, _target.vfxSpawn.EffectRepository.afterimage);
                extraCounter = 0;
            }
        }
    }      

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        currentSpeed = dashJump ? stats.dashJumpVelocityX : stats.movementVelocity;
        controller.LockFlip(true);
        lerpIndex = 0;
        casted = false;
        direction = transform.right;
        if (inputs.FixedAxis.y != 0)
        {
            direction = transform.up * inputs.FixedAxis.y;
        }
        if (!dashJump)
        {
            if (direction.y > 0) _target.QueueAnimation(_target.animations.attackRangeUp.name, false, true);
            else if (direction.y < 0) _target.QueueAnimation(_target.animations.attackRangeDown.name, false, true);
            else _target.QueueAnimation(_target.animations.attackRange.name, false, true);
        }
        else _target.QueueAnimation(_target.animations.attackRangeDash.name, false, true);
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
        dashJump = false;
        controller.LockFlip(false);
        controller.SetAcceleration(inputs.FixedAxis.x != 0 ? 1 : 0);
    }

    protected override void TransitionChecks()
    {
        base.TransitionChecks();
        if(dashJump && controller.Grounded())
        {
            stateDone = true;
        }
    }
    private void Cast()
    {      
        _target.projectileSpawn.Shoot(transform.position, direction, 3, 10, 30, null);
        if (!controller.Grounded())
        {
            controller.SetAcceleration(1);
            if (dashJump)
            {
                controller.LockFlip(false);
                _target.QueueAnimation(_target.animations.dash.name, false, true);
            }
            else _target.QueueAnimation(_target.animations.airPeak.name, false, true);
            if (direction.x != 0)
            {
                if(!dashJump)controller.SetVelocityX(-controller.facingDirection * stats.rangeRecoil);
                controller.SetVelocityY(0);
            }
            else
            {
                controller.SetVelocityY(stats.rangeRecoil * -direction.y);
            }
        }
        else _target.QueueAnimation(_target.animations.idle.name, false, true);

        casted = true;
    }

    private void SetMovement()
    {
        if(!casted)controller.Accelerate(((inputs.FixedAxis.x != 0 ? 1 : -1) / stats.airAccelerationTime) * Time.deltaTime);
    }

    public void dashJumpException()
    {
        dashJump = true;
    }
  
}
