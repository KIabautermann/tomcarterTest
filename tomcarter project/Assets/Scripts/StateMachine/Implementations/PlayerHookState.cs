using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHookState : PlayerUnlockableSkill
{
    
    private bool _hooked;
    private float _distance;
    private Vector3 _startPoint;
    private Vector3 _hookVector;
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
        animationTrigger = stats.hookTrigger;
    }

    protected override void DoChecks()
    {
         base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
        float currentDistance = Vector3.Distance(_startPoint, _hookPoint.position);
        if (!_hooked)
        {
            controller.Accelerate(-1 / stats.groundedAccelerationTime * Time.deltaTime);
            if (currentDistance >= stats.hookMaxDistance)
            {
                stateDone = true;
            }
            else
            {
                _hookPoint.position += _hookVector.normalized * stats.hookSpeed * Time.deltaTime;
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
            else if(controller.Grounded() || controller.OnWall() || controller.OnCeiling())
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
        else{
            controller.SetVelocityX(stats.movementVelocity * controller.lastDirection);
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

        _hookVector = GetAimAssistedHookVector();  
    }

    private Vector3 GetAimAssistedHookVector()
    {
        Vector3 tmpHookVector = new Vector3(stats.hookTarget.x * controller.facingDirection, stats.hookTarget.y, 0) * stats.hookMaxDistance;  
        Vector3 origin = Vector3.zero;
        int rayCount = 10;
        float angle = 0f;
        float angleIncrease = stats.hookAimAssistConeAngle / rayCount;
        float hookDistance = tmpHookVector.magnitude;
        Vector3[] raycastedVectors = new Vector3[rayCount + 1];
        raycastedVectors[0] = tmpHookVector;
        for (int i = 1; i <= rayCount / 2; i++) {
            float currentAngle = angle + i * angleIncrease;
            Vector3 vertexLeft = Quaternion.Euler(0, 0, currentAngle) * tmpHookVector;
            Vector3 vertexRight = Quaternion.Euler(0, 0, -1 * currentAngle) * tmpHookVector;
            raycastedVectors[i * 2 - 1] = vertexLeft;
            raycastedVectors[i * 2] = vertexRight;

            RaycastHit hit;
            if (Physics.Raycast(_startPoint, vertexLeft, out hit, hookDistance, stats.walkable)) {
                return (hit.point - _startPoint).normalized * hookDistance;
            }

            if (Physics.Raycast(_startPoint, vertexRight, out hit, hookDistance, stats.walkable)) {
                return (hit.point - _startPoint).normalized * hookDistance;
            }
        }

        return new Vector3(1 * controller.facingDirection, 1, 0).normalized * stats.hookMaxDistance;
    }

    private Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI/180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
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


    private void OnDrawGizmos() {
        Vector3 facingTarget = new Vector3(stats.hookTarget.x * controller.facingDirection, stats.hookTarget.y ,0);
        Vector3 targetA = (Quaternion.Euler(0,0,stats.hookAimAssistConeAngle) * facingTarget * stats.hookMaxDistance) + transform.position;
        Vector3 targetB = (Quaternion.Euler(0,0,-stats.hookAimAssistConeAngle) * facingTarget * stats.hookMaxDistance) + transform.position;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, targetA);
        Gizmos.DrawLine(transform.position, targetB);
    }
}
