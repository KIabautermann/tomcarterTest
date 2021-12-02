using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRangeState : PlayerAttackState
{
    public float _maxDistance;
    private RaycastHit _raycast;
    private Vector3 _rangeAttackDirection;
    private bool _fromDashJump;
    private bool _hited;
    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        attackDuration = stats.rangeTime;
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
        if (onAir)
        {
            if (!activeHitbox)
            {
                currentAcceleration = _fromDashJump ? stats.airAccelerationTime : stats.dashJumpAccelerationTime;
                currentSpeed = _fromDashJump ? stats.dashJumpVelocityX : stats.movementVelocity;
            }         
        }
        else if(canMove)
        {
            currentAcceleration = stats.groundedAccelerationTime;
            canMove = false;
        }
        controller.FlipCheck(inputs.FixedAxis.x);
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
        if (Physics.Raycast(transform.position, _rangeAttackDirection, out _raycast, stats.rangeHitbox.x, stats.hitable))
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

        if (!activeHitbox) return;

        Vector3 center = transform.position + _rangeAttackDirection * _maxDistance / 2;
        Vector3 size = Hitbox(_rangeAttackDirection)/2;
        Collider[] hitbox = Physics.OverlapBox(center, size, Quaternion.identity, stats.hitable);
        hitDetection = hitbox.Length != 0;
        if (!_hited)
        {
            _hited = true;
            if (hitbox.Length != 0)
            {
                for (int i = 0; i < hitbox.Length; i++)
                {
                    if (hitbox[i].GetComponent<IBounceable>() != null && _rangeAttackDirection == Vector3.down)
                    {
                        GetComponent<PlayerBounceJumpState>().CommingFromDashJump(_fromDashJump);
                        _target.ChangeState<PlayerBounceJumpState>();
                        return;
                    }
                }
            }
            canMove = false;
            controller.SetTotalVelocity(0, controller.CurrentVelocity);
            controller.SetGravity(false);
        }
    }      

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        canMove = true;
        _hited = false;
        _target.SetMaskActive(false);
        currentSpeed = _fromDashJump ? stats.dashJumpVelocityX : stats.movementVelocity;
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
                    _target.QueueAnimation(_target.animations.attackRangeDown.name, true, true);
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
        _fromDashJump = false;
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

    public void ComingFromDashJump()
    {
        _fromDashJump = true;
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
