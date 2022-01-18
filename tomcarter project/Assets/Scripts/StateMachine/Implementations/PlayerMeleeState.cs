using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeState : PlayerAttackState
{
    private bool _combo;
    private int comboCounter;

    [SerializeField]
    private VisualEffectSpawner visualEffectSpawner;
    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        attackDuration = stats.meleeTime;
        animationTrigger = stats.meleeID;
    }

    protected override void DoChecks()
    {
        base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
        if (onAir)
        {
            canMove = true;
            currentAcceleration = stats.airAccelerationTime;
        }
        else
        {
            canMove = false;
            currentAcceleration = stats.groundedAccelerationTime;
        }
        controller.FlipCheck(inputs.FixedAxis.x);
        if(!_combo && !onAir && counter >= (stats.meleeTime - stats.comboWindow) && inputs.MeleeInput && comboCounter <=2)
        {
            _combo = true;
        }
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        
        if (!onAir)
        {
            _target.QueueAnimation(_target.animations.attackGround.name, false, true);
            _target.vfxSpawn.InstanceEffect(gameObject, transform.position, transform.rotation, _target.vfxSpawn.EffectRepository.playerGroundAttack);
        }
        else
        {
            _target.QueueAnimation(_target.animations.attackAir.name, false, true);
            _target.vfxSpawn.InstanceEffect(gameObject, transform.position, transform.rotation, _target.vfxSpawn.EffectRepository.playerAirAttack);
        }
        _combo = false;
    }

    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
        controller.LockFlip(false);
        if (!_combo) comboCounter = 0;
        else comboCounter++;
    }

    protected override void TransitionChecks()
    {      
        base.TransitionChecks();
        if(onAir && controller.Grounded())
        {
            _target.ChangeState<PlayerLandState>();
        }
        if (stateDone && _combo && comboCounter <= 2)
        {
            _target.ChangeState<PlayerMeleeState>();
        }

    }
}
