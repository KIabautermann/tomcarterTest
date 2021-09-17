using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHardenState : PlayerUnlockableSkill
{
    
    private bool _wasGrounded;
    private bool _hasCollided;

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
        base.DoLogicUpdate();
        if(controller.Grounded())
        {
            controller.FlipCheck(inputs.FixedAxis.x);
            controller.Accelerate((inputs.FixedAxis.x != 0 ? 1 / stats.airAccelerationTime : -1 / stats.airAccelerationTime) * Time.deltaTime);
            controller.SetVelocityX(stats.hardenMovementSpeed * controller.facingDirection);                    
        }   
        RaycastHit hit;
        if(Physics.Raycast(controller.myCollider.bounds.center, controller.DirectionalDetection(),out hit, stats.collisionDetection, stats.walkable) && CanBreak())
        {
            if (hit.collider.gameObject.GetComponent<IBreakable>() != null)
            {
                hit.collider.gameObject.GetComponent<IBreakable>().onBreak(FastestAxis());             
            }  
            _hasCollided = true;              
        }  
        if(_wasGrounded!=controller.Grounded() && controller.Grounded()){
            PlayerEventSystem.GetInstance().TriggerPlayerHasLanded(transform.position);
        }                 
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
        if(controller.CurrentVelocity.y < stats.minJumpVelocity)
        {
            controller.Force(Physics.gravity.normalized, stats.fallMultiplier, ForceMode.Force);
        }
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        _target.QueueAnimation(_target.animations.hardenInit.name, false, true);
        _hasCollided = false;
        _wasGrounded=controller.Grounded();
        if(inputs.DashInput || !inputs.DashCancel){
            _target.ChangeState<PlayerDashState>();
        }
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
        controller.SetCollider(stats.colliderDefaultSize, stats.colliderDefaultPosition);
        _target.QueueAnimation(_target.animations.hardenEnd.name, true, true);
    }

    protected override void TransitionChecks()
    {
        base.TransitionChecks();
        if(counter >= stats.hardenTime)
        {
            stateDone = true;
        }      
    }

    bool CanBreak() => controller.CurrentVelocity.magnitude >= stats.minBreakVelocity && !_hasCollided;

    float FastestAxis(){
        if(Mathf.Abs(controller.CurrentVelocity.x) > Mathf.Abs(controller.CurrentVelocity.y)){
            return Mathf.Abs(controller.CurrentVelocity.x);
        }
        else if(controller.CurrentVelocity.y < 0){
            return Mathf.Abs(controller.CurrentVelocity.y);
        }
        else{
            return controller.CurrentVelocity.y * stats.ceilingExceptionMultiplier;
        }
    }

    public void HardenColliderSet()
    {
        controller.SetCollider(stats.colliderHardenSize, stats.colliderHardenPosition);
    }
}
