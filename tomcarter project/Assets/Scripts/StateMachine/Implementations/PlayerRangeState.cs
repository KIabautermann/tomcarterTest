using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRangeState : PlayerAttackState
{
    public float _maxDistance;
    private RaycastHit _raycast;
    private Vector3 _rangeAttackDirection;
    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        attackDuration = stats.rangeTime;
        animationTrigger = stats.rangeID;
    }

    protected override void DoChecks()
    {
        base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
        if (onAir)
        {
            if (!activeHitbox)
            {
                controller.Accelerate((inputs.FixedAxis.x != 0 ? 1 / stats.airAccelerationTime : -1 / stats.airAccelerationTime) * Time.deltaTime);
                if (inputs.JumpCancel && controller.CurrentVelocity.y >= 0 && !forceAplied)
                {
                    controller.SetVelocityY(controller.CurrentVelocity.y * stats.shortHopMultiplier);
                    forceAplied = true;
                }
            }
            else
            {
                controller.SetTotalVelocity(0, Vector2.zero);
                controller.SetAcceleration(0);
                controller.SetGravity(false);
                controller.Accelerate(0);
            }
            
        }
        else
        {
            controller.Accelerate(-1 / stats.groundedAccelerationTime);
        }
        controller.FlipCheck(inputs.FixedAxis.x);
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
        if (controller.CurrentVelocity.y <= stats.minJumpVelocity && !controller.Grounded())
        {
            controller.Force(Physics.gravity.normalized, stats.fallMultiplier, ForceMode.Force);
        }
        if (!activeHitbox)
        {
            controller.SetVelocityX(stats.movementVelocity * controller.lastDirection);
        }
        Vector3 center = transform.position + _rangeAttackDirection * _maxDistance / 2;
        Vector3 size = Hitbox(_rangeAttackDirection)/2;
        Collider[] hitbox = Physics.OverlapBox(center, size, Quaternion.identity, stats.walkable);
        hitDetection = hitbox.Length != 0;
        if (Physics.Raycast(transform.position, _rangeAttackDirection, out _raycast, stats.rangeHitbox.x, stats.walkable))
        {
            _maxDistance = Vector3.Distance(transform.position, _raycast.point);
            _target.SetMaskSize(Vector3.one * 5);
            _target.SetMaskPosition(_raycast.point, _rangeAttackDirection);
            _target.SetMaskActive(true);
        }
        else
        {
            _maxDistance = stats.rangeHitbox.x;
        }
    }      

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        if (!onAir)
        {
            if (inputs.FixedAxis.y == 0)
            {
                _target.QueueAnimation(_target.animations.attackRange.name, false, true);
                _rangeAttackDirection = Vector3.right * controller.facingDirection;
            }
            else if(inputs.FixedAxis.y > 0)
            {
                _target.QueueAnimation(_target.animations.attackRangeUp.name, false, true);
                _rangeAttackDirection = Vector3.up;
            }
        }
        else
        {
            if(inputs.FixedAxis.y == 0)
            {
                _target.QueueAnimation(_target.animations.attackRange.name, false, true);
                _rangeAttackDirection = Vector3.right * controller.facingDirection;
            }
            else
            {
                if(inputs.FixedAxis.y > 0)
                {
                    _target.QueueAnimation(_target.animations.attackRangeUp.name, false, true);
                    _rangeAttackDirection = Vector3.up;
                }
                else
                {
                    _target.QueueAnimation(_target.animations.attackRangeDown.name, false, true);
                    _rangeAttackDirection = -Vector3.up;
                }
            }
            
        }
    }
    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
        controller.LockFlip(false);
        controller.SetGravity(true);
        _target.SetMaskActive(false);
    }

    protected override void TransitionChecks()
    {
        base.TransitionChecks();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = hitDetection ? Color.green : Color.red;
        if (activeHitbox)
        {
            Vector3 size = Hitbox(_rangeAttackDirection);
            Vector3 center = transform.position + _rangeAttackDirection * _maxDistance / 2;
            Gizmos.DrawWireCube(center, size);
            if (hitDetection)
            {
                Gizmos.DrawWireSphere(_raycast.point, .5f);
            }        
        }       
    }

    Vector3 Hitbox(Vector3 direction)
    {
        if(direction == Vector3.right || direction == -Vector3.right)
        {
            return new Vector3(_maxDistance, stats.rangeHitbox.y);
        }
        return new Vector3(stats.rangeHitbox.y, _maxDistance);
    } 
}
