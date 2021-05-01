using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHookState : PlayerSkillState
{
    private bool _hooked;
    private float _distance;
    private Vector3 _startPoint;
    private Vector3 _hookTarget;
    private float dist;    
    private Vector3 _direction;
    private bool _pressedHarden;
    
    #region Init Variables
    private LineRenderer _line;
    private SpriteRenderer _hookSprite;
    private Transform _hookPoint;
    #endregion

    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        _hookPoint = GetComponentInChildren<HookPoint>().transform;
        _line = _hookPoint.GetComponent<LineRenderer>();
        _hookSprite = _hookPoint.GetComponent<SpriteRenderer>();
        _line.enabled = false;
        _hookSprite.enabled = false;
    }

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
                controller.SetVelocityY(controller.CurrentVelocity.y * stats.yVelocityMultiplier);
                stateDone=true;
                controller.SetAcceleration(.5f);
            }
            else if(angle <= stats.maxAngle * controller.facingDirection && controller.facingDirection < 0)
            {                
                controller.SetVelocityY(controller.CurrentVelocity.y * stats.yVelocityMultiplier);
                stateDone=true;
                controller.SetAcceleration(.5f);
            }
            else if(controller.Grounded() || onWall)
            {
                stateDone = true;
                controller.SetTotalVelocity(0, Vector3.zero);
                controller.SetAcceleration(0);
            }
        } 
        if(_line.enabled)
        {
            _line.SetPosition(0,transform.position);   
            _line.SetPosition(1,_hookPoint.position); 
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
        if (inputs.FixedAxis.x != 0)
        {
            controller.SetAcceleration(.5f);
        }
        else
        {
            controller.SetAcceleration(0);
        }
        _line.SetPosition(0,transform.position);   
        _line.SetPosition(1,_hookPoint.position); 
        _line.enabled = true;
        _hookSprite.enabled = true;      
    }

    
    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
        controller.SetGravity(true);
        _hookPoint.parent = _target.transform;
        _line.enabled = false;
        _hookSprite.enabled = false;
    }

    protected override void TransitionChecks()
    {
        base.TransitionChecks();   
        if(!inputs.GuardCancel && stateDone){
            _target.ChangeState<PlayerHardenState>();
            controller.SetVelocityX(0);
            controller.SetAcceleration(0);
            controller.SetVelocityY(controller.CurrentVelocity.y * stats.hardenHookMultiplier);
        }      
    }

}
