using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeState : PlayerAttackState
{

    private bool _air;
    private bool _forceAplied;
    
    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        hitbox = stats.meleeHitbox;
        hitboxOffset = stats.meleeHiboxOffset;
        animationTrigger = stats.meleeID;
        stateIndex = stats.meleeNumberID;
    }

    
    protected override void DoChecks()
    {
        base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
        if (_air)
        {
            controller.Accelerate((inputs.FixedAxis.x != 0 ? 1 / stats.airAccelerationTime : -1 / stats.airAccelerationTime) * Time.deltaTime);
            if (inputs.JumpCancel && controller.CurrentVelocity.y >= 0 && !_forceAplied)
            {
                controller.SetVelocityY(controller.CurrentVelocity.y * stats.shortHopMultiplier);
                _forceAplied = true;
            }
        }
        else
        {
            controller.Accelerate(-1 / stats.groundedAccelerationTime);
        }
        controller.FlipCheck(inputs.FixedAxis.x);
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
        if (controller.CurrentVelocity.y <= stats.minJumpVelocity && !controller.Grounded())
        {
            controller.Force(Physics.gravity.normalized, stats.fallMultiplier, ForceMode.Force);
        }
        controller.SetVelocityX(stats.movementVelocity * controller.lastDirection);
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        controller.LockFlip(true);
        _forceAplied = false;
        if (controller.Grounded())
        {
            _target.QueueAnimation(_target.animations.attackGround.name, false, true);
            _air = false;
        }
        else
        {
            _target.QueueAnimation(_target.animations.attackAir.name, false, true);
            _air = true;
            isLocked = true;
        }
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
        controller.LockFlip(false);
    }

    protected override void TransitionChecks()
    {
        base.TransitionChecks();
        if (counter >= stats.meleeTime)
        {
            stateDone = true;
        }
        if(_air && controller.Grounded())
        {
            _target.ChangeState<PlayerLandState>();
        }
    }

    public void Unlock()
    {
        isLocked = false;
    }

    private void OnDrawGizmos() {
        if(activeHitbox){
            Vector3 offset = new Vector3(hitboxOffset.x * controller.facingDirection, hitboxOffset.y,0);
            Gizmos.DrawWireCube(transform.position + offset, hitbox);
        }
    }
}
