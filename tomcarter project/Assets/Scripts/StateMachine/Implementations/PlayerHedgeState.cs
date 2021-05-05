using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHedgeState : PlayerUnlockableSkill
{
    private Vector3 _direction;
    private bool _exitingHedge;
    private bool _enteringHedge;
    private float _currentSpeed;

    protected override void DoChecks()
    {
        base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
        if(_enteringHedge)
        {
            controller.Accelerate(-1 / stats.groundedAccelerationTime * Time.deltaTime);
            _currentSpeed = stats.hedgeTransitionInPush;
            if(controller.CurrentVelocity.magnitude <=stats.hedgeTransitionInMinSpeed)
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
        if(check.Length == 0)
        {
            _exitingHedge = true;
            controller.SetTotalVelocity(0, Vector3.zero);
            controller.Force(_direction,stats.hedgeTransitionOutPush,ForceMode.Force);
        }
        else
        {
            _exitingHedge = false;
        }  
        controller.SetTotalVelocity(_currentSpeed, _direction);    
    }

    protected override void DoTransitionIn()
    {   
        base.DoTransitionIn();
        PlayerEventSystem.GetInstance().TriggerPlayerEnteredHedge();
        controller.SetAcceleration(1);
        _direction = controller.CurrentVelocity.normalized;  
        Physics.IgnoreLayerCollision(9,10,true);
        _exitingHedge = false;
        _enteringHedge = true;         
        controller.SetGravity(false);       
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
        controller.SetGravity(true);
        Physics.IgnoreLayerCollision(9,10,false);
        if(_direction.x != 0)
        {
            controller.SetAcceleration(1);
        }
        else
        {
            controller.SetAcceleration(0);
        }
        if(!controller.Grounded())
        {
            airState.JumpCoyoteTimeStart();
        }
    }

    protected override void TransitionChecks()
    {
        base.TransitionChecks(); 
        if(_exitingHedge)
        {
            if(inputs.JumpInput)
            {
                _target.ChangeState<PlayerJumpState>();
            inputs.UsedJump();
                airState.DashJumpCoyoteTimeStart();
            }
            else
            {
                Collider[] check = Physics.OverlapBox(controller.myCollider.bounds.center, controller.myCollider.bounds.size/2, Quaternion.identity,stats.hedge);  
                if(check.Length == 0)
                {                  
                    stateDone = true;
                }  
            }           
        }         
    } 
}
