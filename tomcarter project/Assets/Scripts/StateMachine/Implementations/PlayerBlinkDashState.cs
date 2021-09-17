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
        if(counter >= stats.blinkDashInitTime && !blinkStarted)
        {
            currentSpeed = stats.blinkDashSpeed;
        }
        if (counter >= stats.blinkDashInitTime + stats.blinkDashTime)
        {
            stateDone = true;
        }
    }

    protected override void InstanceAfterImage()
    { 
        if(counter >= stats.blinkDashInitTime)
        {
            ComponentCache<MonoBehaviour> afterImageComponents = sporeTrailPooler.GetItem(Vector3.zero, Quaternion.identity);
            afterImageComponents.GetInstance(typeof(SporeTrailEffect), out MonoBehaviour tmp);
            sporeTrailEffect = tmp as SporeTrailEffect;

            sporeTrailEffect.gameObject.transform.SetParent(gameObject.transform);
            sporeTrailEffect.LogicStart();
        }      
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();
        currentSpeed = 0;
        _target.QueueAnimation(_target.animations.blinkInit.name, false, true);
        if(inputs.FixedAxis != Vector2.zero)
        {
            direction = inputs.FixedAxis;
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
        
        if (controller.CurrentVelocity.y != 0)
        {
            controller.SetVelocityY(controller.CurrentVelocity.y * stats.dashEndMultiplier);
        }
        if(inputs.FixedAxis.x != 0)
        {
            controller.SetAcceleration(1);
        }
        else
        {
            controller.SetAcceleration(0);
        }
        
        if (sporeTrailEffect != null) {
            sporeTrailEffect.LogicEnd();
            sporeTrailEffect = null;
        }

        _target.QueueAnimation(_target.animations.blinkEnd.name, true, true);
    }
    protected override void TransitionChecks()
    {
        base.TransitionChecks();
    }
}
