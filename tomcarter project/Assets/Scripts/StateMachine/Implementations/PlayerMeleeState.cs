using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeState : PlayerAttackState
{
    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        startupTime = stats.meleeStartupTime;
        hitboxTime = stats.meleeHitboxTime;
        recoveryTime = stats.meleerecoveryTime;
        hitbox = stats.meleeHitbox;
        hitboxOffset = stats.meleeHiboxOffset;
        animationTrigger = stats.meleeTrigger;
        asynchronous = true;
    }

    
    protected override void DoChecks()
    {
        base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
        if(counter >= startupTime + hitboxTime + recoveryTime){
            StateEnd();  
        }
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
    }

    protected override void TransitionChecks()
    {
        base.TransitionChecks();
    }

    private void OnDrawGizmos() {
        if(activeHitbox){
            Vector3 offset = stats.meleeHiboxOffset;
            Gizmos.DrawWireCube(transform.position + offset, hitbox);
        }
    }

    public void StateEnd(){
        _target.removeSubState();
    }
}
