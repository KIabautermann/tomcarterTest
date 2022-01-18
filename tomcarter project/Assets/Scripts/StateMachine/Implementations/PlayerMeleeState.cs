using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeState : PlayerTransientState
{
    private bool _combo;
    private int comboCounter;
    private bool casted;
    [Range (0,1)]
    public float index;
    private bool onAir;
    private bool backslash;
    private float attackDuration;
    private bool combo;

    [SerializeField]
    private VisualEffectSpawner visualEffectSpawner;
    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        animationTrigger = stats.meleeID;
    }

    protected override void DoChecks()
    {
        base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
        if (counter > attackDuration)
        {
            if(!onAir && combo)
            {
                _target.ChangeState<PlayerMeleeState>();
            }
            else stateDone = true;
        }
        if (onAir)
        {
            if (counter > stats.airAnticipationTime && !casted) CastAirAttack();
            else if(casted) index += 2 / stats.airRecoveryTime * Time.deltaTime;
        }
        if (inputs.MeleeInput && counter > stats.groundAnticipationTime + stats.groundRecoverytime - stats.comboWindow)
        {
            combo = true;
        }
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
        if (casted && onAir)
        {
            controller.SetAcceleration(1);
            controller.SetVelocityX(Mathf.Lerp(stats.airAttackBoost, 0, index) * controller.facingDirection);
        }
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        combo = false;
        casted = false;
        index = 0;
        onAir = !controller.Grounded();
        attackDuration = onAir ? stats.airAnticipationTime + stats.airRecoveryTime : stats.groundAnticipationTime + stats.groundRecoverytime;
        if (onAir)
        {
            controller.SetGravity(false);
            controller.SetTotalVelocity(controller.CurrentVelocity.magnitude * .2f, controller.CurrentVelocity.normalized);
            _target.QueueAnimation(_target.animations.attackAir.name, false, true);
            _target.vfxSpawn.InstanceEffect(gameObject, transform.position, transform.rotation, _target.vfxSpawn.EffectRepository.playerAirAttack);
        }
        else
        {         
            controller.SetVelocityX(0);
            _target.QueueAnimation(_target.animations.attackGround.name, false, true);
            if (backslash) _target.vfxSpawn.InstanceEffect(gameObject, transform.position, transform.rotation, _target.vfxSpawn.EffectRepository.playerGroundAttackB);
            else _target.vfxSpawn.InstanceEffect(gameObject, transform.position, transform.rotation, _target.vfxSpawn.EffectRepository.playerGroundAttack);
            backslash = !backslash;
        }
    }

    protected override void DoTransitionOut()
    {
        controller.SetGravity(true);
        controller.SetAcceleration(0);
        if (onAir)
        {
            if (onAir) _target.GravityExceptionTime();
            coolDown = stats.airCooldown;
        }
        else coolDown = 0;    
    }

    protected override void TransitionChecks()
    {

        base.TransitionChecks();
    }


    private void CastGroundAttack()
    {
        casted = true;
    }

    private void CastAirAttack()
    {     
        casted = true;
        controller.SetVelocityY(0);
    }

    public void ResetCooldown()
    {
        coolDown = 0;
    }
}
