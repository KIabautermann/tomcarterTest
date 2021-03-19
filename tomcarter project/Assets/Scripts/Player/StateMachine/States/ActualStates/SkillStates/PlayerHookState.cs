using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHookState : PlayerSkillState
{
    private bool _hooked;
    private float _distance;
    private Vector3 _startPoint;
    private Vector3 _target;
    private float dist;
    public Vector3 direction;
    private ChainVIsuals hookVisuals;
    private float _lastHookTime;
    public PlayerHookState(PlayerController player, PlayerStateMachine stateMachine, PlayerData playerData, string currentAnimation) : base(player, stateMachine, playerData, currentAnimation)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if(hookVisuals == null)
        {
            hookVisuals = player.hookPoint.GetComponent<ChainVIsuals>();
        }
        _hooked = false;
        player.hookPoint.position = player.transform.position;       
        player.hookPoint.parent = null;
        _startPoint = player.transform.position;
        hookVisuals.TurnRenderers(true);
        player.MyInputs.Hooked();
        if (axis.x != 0)
        {
            player.SetAcceleration(.5f);
        }
        else
        {
            player.SetAcceleration(0);
        }      
    }

    public override void Exit()
    {
        base.Exit();
        player.RB.useGravity = true;
        if(axis.x != 0)
        {
            player.SetAcceleration(.5f);
        }
        hookVisuals.TurnRenderers(false);
        player.hookPoint.parent = player.transform;
        HookTime();
    }

    public override void LogicUpdate()
    {

        base.LogicUpdate();
        _target = new Vector3(playerData.hookTarget.x * player.facingDirection, playerData.hookTarget.y, 0);
        player.Accelerate(-1 / playerData.groundedAccelerationTime * Time.deltaTime);
        player.SetVelocityX(playerData.movementVelocity * player.facingDirection);
        float currentDistance = Vector3.Distance(_startPoint, player.hookPoint.position);
        if (!_hooked)
        {
            if (currentDistance >= playerData.hookTarget.magnitude)
            {
                abilityDone = true;
            }
            else
            {
                player.hookPoint.position += _target.normalized * playerData.hookSpeed * Time.deltaTime;
                Collider[] hookDetecion = Physics.OverlapSphere(player.hookPoint.position, playerData.hookDetectionRadius, playerData.walkable);
                if(hookDetecion.Length > 0)
                {
                    if (grounded)
                    {
                        abilityDone = true;
                    }
                    else
                    {
                        _hooked = true;
                        dist = Vector3.Distance(player.hookPoint.position, player.transform.position);
                        player.SetTotalVelocity(0,Vector3.zero);
                        player.RB.useGravity = false;
                    }                  
                }
            }
        }
        else
        {
            direction = (player.hookPoint.position - player.transform.position).normalized;
            Quaternion rotation = Quaternion.Euler(0, 0, -90 * player.facingDirection);
            direction = rotation * direction;
            Vector3 target = player.transform.position + direction;
            target = target - player.hookPoint.transform.position;
            target = player.hookPoint.position + target.normalized * dist;
            direction = (target - player.transform.position).normalized;
            float angle = Vector3.SignedAngle(Vector3.up, (player.hookPoint.position - player.transform.position).normalized, Vector3.forward);
            Debug.Log(angle);
            if (angle >= playerData.maxAngle * player.facingDirection && player.facingDirection > 0)
            {
                abilityDone = true;
                player.SetAcceleration(1);
            }
            else if(angle <= playerData.maxAngle * player.facingDirection && player.facingDirection < 0)
            {
                abilityDone = true;
                player.SetAcceleration(1);
            }
            else if(grounded || onWall)
            {
                abilityDone = true;
                player.SetTotalVelocity(0, Vector3.zero);
                player.SetAcceleration(0);
            }
        }
    }

    public bool CanHook()
    {
        return Time.time >= _lastHookTime + playerData.hookCooldown;
    }

    private void HookTime()
    {
        _lastHookTime = Time.time;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (_hooked)
        {
            player.SetTotalVelocity(playerData.circularSpeed * dist, direction);
        }

    }  
}
