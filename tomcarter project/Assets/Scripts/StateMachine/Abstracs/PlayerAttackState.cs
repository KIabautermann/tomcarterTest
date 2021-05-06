using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerSkillState
{
    protected float startupTime;
    protected float hitboxTime;
    protected float recoveryTime;
    protected bool activeHitbox;
    protected Vector2 hitbox;
    protected Vector3 hitboxOffset;
    protected bool chainAttack;
    
    protected override void DoChecks()
    {
        base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
        if(Time.time >= startTime + startupTime && !activeHitbox){
            activeHitbox = true;
        }
        if(Time.time >= startTime + startupTime + hitboxTime && activeHitbox){
            activeHitbox = false;
        }
        if(Time.time >= startTime + startupTime + hitboxTime + recoveryTime && !chainAttack){
            stateDone = true;
        }       
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        chainAttack = false;
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
    }

    protected override void OnDestroyHandler()
    {
        base.OnDestroyHandler();
    }

    protected override void TransitionChecks()
    {
        base.TransitionChecks();
    }
}
