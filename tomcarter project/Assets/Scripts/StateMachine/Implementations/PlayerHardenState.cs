using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHardenState : PlayerBasicMovementState
{
    
    private bool _groundPound;
    private bool _startedGroundPound;

    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        animationTrigger = stats.hardenID;
        stateIndex = stats.hardenNumberID;
    }

    public override string ToString()
    {
        return base.ToString();
    }

    protected override void DoChecks()
    {
        base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        if (!_startedGroundPound)
        {
            base.DoLogicUpdate();
        }
        else Debug.Log(controller.CurrentVelocity.y);
    }

    protected override void DoPhysicsUpdate()
    {
        if (!_startedGroundPound)
        {
            base.DoPhysicsUpdate();
        }
        else
        {
            Vector3 checkPosition = controller.myCollider.bounds.center + controller.DirectionalDetection() * stats.hedgeDetectionOffset;
            Collider[] check = Physics.OverlapBox(checkPosition, controller.myCollider.bounds.size/2, Quaternion.identity,stats.walkable);
            if(check.Length != 0)
            {        
                stateDone = true;
            }
        }
        
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        _target.QueueAnimation(_target.animations.hardenInit.name, false, true);
        controller.SetCollider(stats.colliderHardenSize, stats.colliderHardenPosition);
        _groundPound = !controller.Grounded() && inputs.FixedAxis.y < 0;
        _startedGroundPound = false;
        currentSpeed = stats.movementVelocity;
        currentAcceleration = stats.airAccelerationTime;
        if (_groundPound)
        {
            controller.SetVelocityY(controller.CurrentVelocity.y / 2);
            controller.SetGravity(false);
        }
        canMove = false;
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
        controller.SetCollider(stats.colliderDefaultSize, stats.colliderDefaultPosition);
        _target.QueueAnimation(_target.animations.hardenEnd.name, true, true);
        controller.SetGravity(true);
        if (_groundPound)
        {
            controller.SetTotalVelocity(0, Vector2.zero);
            controller.FlipCheck(inputs.FixedAxis.x);
            controller.SetAcceleration(inputs.FixedAxis.x != 0 ? .5f : 0);        
            controller.Force(Vector2.up, 10, ForceMode.Impulse);
        }
        else
        {
            controller.SetAcceleration(0);
        }
    }

    protected override void TransitionChecks()
    {
        base.TransitionChecks();
        if(counter >= stats.hardenTime && !_groundPound)
        {
            stateDone = true;
        }      
    } 

    public void HardenColliderSet()
    {
        controller.SetCollider(stats.colliderHardenSize, stats.colliderHardenPosition);
    }

    public void StartGroundPound()
    {
        if (_groundPound)
        {
            _startedGroundPound = true;
            controller.SetAcceleration(1);
            controller.SetTotalVelocity(25, -Vector2.up);
        }       
    }
}
