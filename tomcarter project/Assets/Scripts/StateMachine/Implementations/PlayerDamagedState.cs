using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerDamagedState : PlayerTransientState
{
    private Respawner respawner;
    private Vector3 lastSafeZone;
    private float velocity;
    private bool hazardCollision;
    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        this.respawner = GetComponent<Respawner>();
        animationTrigger = stats.damageTrigger;
    }
    protected override void DoChecks()
    {
        base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        if (hazardCollision) {
            Vector3 direction = Vector3.Normalize(lastSafeZone - gameObject.transform.position);
            controller.SetAcceleration(1f);
            controller.SetTotalVelocity(velocity, direction);
        }
    }

    protected override void DoPhysicsUpdate()
    {
        
    }

    protected override void DoTransitionIn()
    {
        controller.SetTotalVelocity(0f, Vector2.right);
        hazardCollision = false;
        base.DoTransitionIn();
    }

    protected override void DoTransitionOut()
    {
        controller.SetTotalVelocity(0f, Vector2.right);
        controller.SetAcceleration(0f);
        controller.SetGravity(true);
        controller.myCollider.enabled = true;
        base.DoTransitionOut();
    }

    protected override void TransitionChecks()
    {

        if (hazardCollision) {
            if (Vector3.Magnitude(gameObject.transform.position - lastSafeZone) < 0.01f) {
                // animacion de aparecer
                _target.ChangeState<PlayerIdleState>();
            }
            return;
        }

        Collider[] _hazardHit = Physics.OverlapBox(
            transform.position, 
            controller.myCollider.bounds.size/2 * 1.1f,
            Quaternion.identity, stats.hazard);

        playerHealth.currentHealth--;

        if (_hazardHit.Length > 0) {
            PlayerEventSystem.GetInstance().TriggerPlayerCollidedHazard();
            
            lastSafeZone = respawner.LastSafeZone;
            hazardCollision = true;
            controller.myCollider.enabled = false;
            controller.SetGravity(false);
            velocity = Vector3.Distance(gameObject.transform.position, lastSafeZone);
            // disparar animacion de desaparecer
        } else {
            stateDone = true;
            base.TransitionChecks();
        }
    }

}
