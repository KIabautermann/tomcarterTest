using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBlinkDashState : PlayerDashState
{
    private ObjectPooler sporeTrailPooler;
    private SporeTrailEffect sporeTrailEffect;
    private bool blinkStarted;

    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        animationTrigger = stats.blinkID;
        sporeTrailPooler = target.sporeTrailPooler;
    }
    protected override void DoChecks()
    {
        base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        base.DoLogicUpdate();
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        _target.QueueAnimation(_target.animations.dash.name, false, true);
        if(inputs.FixedAxis != Vector2.zero)
        {
            direction = new Vector3(inputs.FixedAxis.x, inputs.FixedAxis.y,0);
            direction = direction.normalized;
        }
        else
        {
            direction = new Vector2(controller.facingDirection,0);
        } 
    }
    protected override void DoTransitionOut()
    {
        base.DoTransitionOut();
                    
        if (sporeTrailEffect != null) {
            sporeTrailEffect.LogicEnd();
            sporeTrailEffect = null;
        }
    }
    protected override void TransitionChecks()
    {
        base.TransitionChecks();
    }
}
