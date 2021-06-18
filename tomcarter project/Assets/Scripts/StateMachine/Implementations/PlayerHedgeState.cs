using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHedgeState : PlayerUnlockableSkill
{
    private Vector3 _direction;
    private bool _exitingHedge;
    private bool _enteringHedge;
    private float _currentSpeed;
    private bool _hedgeJumpCoyoteTime;
    public float _hedgeJumpStart;

    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        animationTrigger = stats.hedgeID;
    }

    protected override void DoChecks()
    {
        base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
        HedgeJumpCoyoteTimeCheck();
        if(_enteringHedge)
        {
            _currentSpeed = stats.hedgeTransitionInPush;
            if(counter >= stats.hedgeTransitionInTime)
            {
                _enteringHedge = false;
            }
        }
        else if(_exitingHedge)
        {
            controller.SetAcceleration(1);
            _currentSpeed = stats.hedgeTransitionOutPush;
        }
        else if(!_exitingHedge && !_enteringHedge)
        {
            _currentSpeed = stats.movementVelocity;
            if(inputs.FixedAxis.magnitude !=0)
            {
                _direction = new Vector3(inputs.FixedAxis.x, inputs.FixedAxis.y,0).normalized;
            }     
            controller.FlipCheck(inputs.FixedAxis.x);
            controller.Accelerate((inputs.FixedAxis.magnitude != 0 ? 1 / stats.groundedAccelerationTime : -1 / stats.groundedAccelerationTime) * Time.deltaTime);
        }       
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
        Vector3 checkPosition = controller.myCollider.bounds.center + controller.DirectionalDetection() * stats.hedgeDetectionOffset;
        Collider[] check = Physics.OverlapBox(checkPosition, controller.myCollider.bounds.size/2, Quaternion.identity,stats.hedge);  
        if(!_enteringHedge && check.Length == 0)
        {
            _exitingHedge = true;
            controller.SetTotalVelocity(0, Vector3.zero);
            controller.Force(_direction,stats.hedgeTransitionOutPush,ForceMode.Force);
        }
        
        controller.SetTotalVelocity(_currentSpeed, _direction);    
    }

    protected override void DoTransitionIn()
    {   
        base.DoTransitionIn();
        PlayerEventSystem.GetInstance().TriggerPlayerEnteredHedge();
        controller.SetAcceleration(1);
        _direction = controller.CurrentVelocity.normalized;  
        _exitingHedge = false;
        _enteringHedge = true;        
        _hedgeJumpCoyoteTime = false; 
        Physics.IgnoreLayerCollision(9,10,true);
        controller.SetGravity(false);   
        _target.removeSubState();
        controller.SetDrag(0);    
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
        controller.SetGravity(true);
        Physics.IgnoreLayerCollision(9,10,false);
        if(_direction.x != 0)
        {
            controller.SetTotalVelocity(_currentSpeed, new Vector2(_direction.x, 0));
            controller.SetAcceleration(1);
        }
        else
        {
            controller.SetTotalVelocity(_currentSpeed, new Vector2(0, _direction.y));
            controller.SetAcceleration(0);
        }
    }

    protected override void TransitionChecks()
    {
        base.TransitionChecks(); 
        if (inputs.JumpInput) 
        {
            _hedgeJumpStart = counter;
            _hedgeJumpCoyoteTime = true; 
        }
        if (inputs.JumpCancel)
        {
            _hedgeJumpCoyoteTime = false; 
        }
        if(_exitingHedge)
        {          
            Collider[] check = Physics.OverlapBox(transform.position, controller.myCollider.bounds.size/2, Quaternion.identity,stats.hedge);  
            if(check.Length == 0)
            {                  
                if (_hedgeJumpCoyoteTime) 
                {
                    _target.ChangeState<PlayerJumpState>();
                }
                stateDone = true;
            }           
        }      
    }
    private void HedgeJumpCoyoteTimeCheck()
    {
        if (_hedgeJumpCoyoteTime && counter > (stats.hedgeJumpHandicapTime + _hedgeJumpStart))
        {
            _hedgeJumpCoyoteTime = false;
        }
    } 
}
