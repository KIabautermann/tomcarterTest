using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHardenState : PlayerBasicMovementState
{
    
    private bool _groundPound;
    private bool _startedGroundPound;
    private bool _impacted;

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
        if (!_startedGroundPound ||_impacted)
        {
            base.DoLogicUpdate();
        }
        else Debug.Log(controller.CurrentVelocity.y);
    }

    protected override void DoPhysicsUpdate()
    {
        if (!_startedGroundPound || _impacted)
        {
            base.DoPhysicsUpdate();
        }
        else
        {
            Vector3 checkPosition = controller.myCollider.bounds.center + controller.DirectionalDetection() * stats.hedgeDetectionOffset;
            Collider[] check = Physics.OverlapBox(checkPosition, controller.myCollider.bounds.size/2, Quaternion.identity,stats.walkable);
            if(check.Length != 0 && !_impacted)
            {
                controller.SetTotalVelocity(0, Vector2.zero);
                controller.SetVelocityY(stats.jumpVelocity);
                _impacted = true;
                canMove = true;
                controller.FlipCheck(inputs.FixedAxis.x);
                _target.QueueAnimation(_target.animations.hardenBounce.name, true, true);
                controller.SetGravity(true);
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
        canShortHop = false;
        _impacted = false;
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
        controller.SetCollider(stats.colliderDefaultSize, stats.colliderDefaultPosition);
        if(!_groundPound)_target.QueueAnimation(_target.animations.hardenEnd.name, true, true);
        controller.SetGravity(true);
        controller.SetAcceleration(inputs.FixedAxis.x != 0 ? .5f : 0);
    }

    protected override void TransitionChecks()
    {
        base.TransitionChecks();
        if(counter >= stats.hardenTime && !_groundPound)
        {
            stateDone = true;
        } 
        else if(_impacted && controller.CurrentVelocity.y <= 0)
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
            _target.QueueAnimation(_target.animations.hardenPound.name, false, true);
        }       
    }
}
