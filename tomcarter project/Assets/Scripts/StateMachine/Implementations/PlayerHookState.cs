using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHookState : PlayerSkillState
{
    private bool _hooked;
    private bool _hardenInputPressed;
    private bool _readyToHarden;
    private float _distance;
    private Vector3 _startPoint;
    private Vector3 _hookTarget;
    private float dist;
    [SerializeField]
    private Vector3 _direction;
    [SerializeField]
    private Transform _hookPoint;

    protected override void DoChecks()
    {
         base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
        _hookTarget = new Vector3(stats.hookTarget.x * controller.facingDirection, stats.hookTarget.y, 0);       
        float currentDistance = Vector3.Distance(_startPoint, _hookPoint.position);
        if (!_hooked)
        {
            controller.Accelerate(-1 / stats.groundedAccelerationTime * Time.deltaTime);
            controller.SetVelocityX(stats.movementVelocity * controller.facingDirection);
            if (currentDistance >= stats.hookTarget.magnitude)
            {
                stateDone = true;
            }
            else
            {
                _hookPoint.position += _hookTarget.normalized * stats.hookSpeed * Time.deltaTime;
                Collider[] hookDetecion = Physics.OverlapSphere(_hookPoint.position, stats.hookDetectionRadius, stats.walkable);
                if(hookDetecion.Length > 0)
                {
                    if (controller.Grounded())
                    {
                        stateDone = true;
                    }
                    else
                    {
                        _hooked = true;
                        controller.SetAcceleration(1);
                        dist = Vector3.Distance(_hookPoint.position, _target.transform.position);
                        controller.SetTotalVelocity(0,Vector3.zero);
                        controller.SetGravity(false);
                    }                  
                }
            }
        }
        else
        {
            _direction = (_hookPoint.position - _target.transform.position).normalized;
            Quaternion rotation = Quaternion.Euler(0, 0, -90 * controller.facingDirection);
            _direction = rotation * _direction;
            Vector3 target = _target.transform.position + _direction;
            target = target - _hookPoint.transform.position;
            target = _hookPoint.position + target.normalized * dist;
            _direction = (target - _target.transform.position).normalized;
            float angle = Vector3.SignedAngle(Vector3.up, (_hookPoint.position - _target.transform.position).normalized, Vector3.forward);
            if (angle >= stats.maxAngle * controller.facingDirection && controller.facingDirection > 0)
            {               
                controller.SetAcceleration(1);
                stateDone=true;
            }
            else if(angle <= stats.maxAngle * controller.facingDirection && controller.facingDirection < 0)
            {                
                controller.SetAcceleration(1);
                stateDone=true;
            }
            if (angle >= stats.minHookAngle * controller.facingDirection && controller.facingDirection > 0)
            {
                _readyToHarden = true;
                
            }
            else if(angle <= stats.minHookAngle * controller.facingDirection && controller.facingDirection < 0)
            {
                _readyToHarden = true;
            }
            else if(controller.Grounded() || onWall)
            {
                stateDone = true;
                controller.SetTotalVelocity(0, Vector3.zero);
                controller.SetAcceleration(0);
            }
            if(inputs.GuardInput)
            {
                _hardenInputPressed = true;
            }  
        }       
    }
    

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
        if (_hooked)
        {
            controller.SetTotalVelocity(stats.circularSpeed * dist, _direction);
        }
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        _hooked = false;
        _hookPoint.position = _target.transform.position;       
        _hookPoint.parent = null;
        _startPoint = _target.transform.position;
        _readyToHarden = false;
        _hardenInputPressed = false;
        if (inputs.FixedAxis.x != 0)
        {
            controller.SetAcceleration(.5f);
        }
        else
        {
            controller.SetAcceleration(0);
        }      
    }

    
    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
        controller.SetGravity(true);
        if(inputs.FixedAxis.x != 0)
        {
            controller.SetAcceleration(.5f);
        }
        _hookPoint.parent = _target.transform;
    }

    protected override void TransitionChecks()
    {
        if(_readyToHarden && _hardenInputPressed)
        {
            controller.SetTotalVelocity(controller.CurrentVelocity.magnitude,_direction);
            _target.ChangeState<PlayerHardenState>();          
            inputs.UsedGuard();
        }
        else
        {
            base.TransitionChecks();
        }      
    }
}
