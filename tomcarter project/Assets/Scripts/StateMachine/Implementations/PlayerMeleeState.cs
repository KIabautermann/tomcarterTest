using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeState : PlayerAttackState
{
    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        asynchronous = true;
        startupTime = stats.meleeStartupTime;
        hitboxTime = stats.meleeHitboxTime;
        recoveryTime = stats.meleerecoveryTime;
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
        if(endByAnimation){
            stateDone = true;
            _target.removeSubState();
        }
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
        
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        controller.LockFlip(true);
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
        controller.LockFlip(false);
    }

    protected override void TransitionChecks()
    {
        base.TransitionChecks();
    }

    private void OnDrawGizmos() {
        if(activeHitbox){
            Vector3 offset = new Vector3(hitboxOffset.x * controller.facingDirection, hitboxOffset.y,0);
            Gizmos.DrawWireCube(transform.position + offset, hitbox);
        }
    }
}
