using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHookState : PlayerUnlockableSkill
{
    
    private bool _hooked;
    private bool _postHookBoost;
    private float _distance;
    private Vector3 _startPoint;
    private Vector3 _hookVector;
    private float dist;    
    private Vector3 _direction;

    #region Init Variables
    private SpriteRenderer _hookSprite;
    private Transform _hookPoint;
    #endregion

    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        _hookPoint = GetComponentInChildren<HookPoint>().transform;
        _hookSprite = _hookPoint.GetComponent<SpriteRenderer>();
        _hookSprite.enabled = false;
        animationTrigger = stats.hookID;
        stateIndex = stats.hookNumberID;
        coolDown = stats.hookCooldown;
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
                        animationIndex = 1;
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
                stateDone =true;
                _postHookBoost = true;
            }
            else if(angle <= stats.maxAngle * controller.facingDirection && controller.facingDirection < 0)
            {                
                stateDone =true;
                _postHookBoost = true;
            }
            else if(controller.Grounded() || controller.OnWall() || controller.OnCeiling())
            {
                stateDone = true;
                controller.SetTotalVelocity(0, Vector3.zero);
                controller.SetAcceleration(0);
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
        else{
            controller.SetVelocityX(stats.movementVelocity * controller.lastDirection);
        }
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();    

        _hooked = false;
        _postHookBoost = false;
        _hookPoint.position = _target.transform.position;       
        _hookPoint.parent = null;
        _startPoint = _target.transform.position;
        if(controller.CurrentVelocity.y > 0)
        {
            controller.SetVelocityY(controller.CurrentVelocity.y / 2);
        }
        if (inputs.FixedAxis.x != 0)
        {
            controller.SetAcceleration(.5f);
        }
        else
        {
            controller.SetAcceleration(0);
        }
        _hookSprite.enabled = true;      

        _hookVector = GetAimAssistedHookVector();  
    }

    private Vector3 GetAimAssistedHookVector()
    {
        Vector3 tmpHookVector = new Vector3(stats.hookTarget.x * controller.facingDirection, stats.hookTarget.y, 0).normalized * stats.hookMaxDistance;  
        Vector3 origin = Vector3.zero;
        int rayCount = 10;
        float angle = 0f;
        float angleIncrease = stats.hookAimAssistConeAngle / rayCount;
        float hookDistance = tmpHookVector.magnitude;
        for (int i = 1; i <= rayCount / 2; i++) {
            float currentAngle = angle + i * angleIncrease;
            Vector3 vertexLeft = Quaternion.Euler(0, 0, currentAngle) * tmpHookVector;
            Vector3 vertexRight = Quaternion.Euler(0, 0, -1 * currentAngle) * tmpHookVector;
            
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
        _hookSprite.enabled = false;
        if (_postHookBoost)
        {
            controller.SetVelocityY(controller.CurrentVelocity.y * stats.yVelocityMultiplier);
            controller.SetAcceleration(0f);
        }
    }

    protected override void TransitionChecks()
    {
        base.TransitionChecks();   
    }

    private void OnDrawGizmos() {
        if (stats == null || controller == null) return;
        
        Vector3 facingTarget = new Vector3(stats.hookTarget.x * controller.facingDirection, stats.hookTarget.y ,0);
        Vector3 targetA = (Quaternion.Euler(0,0,stats.hookAimAssistConeAngle) * facingTarget * stats.hookMaxDistance) + transform.position;
        Vector3 targetB = (Quaternion.Euler(0,0,-stats.hookAimAssistConeAngle) * facingTarget * stats.hookMaxDistance) + transform.position;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, targetA);
        Gizmos.DrawLine(transform.position, targetB);
    }
}
