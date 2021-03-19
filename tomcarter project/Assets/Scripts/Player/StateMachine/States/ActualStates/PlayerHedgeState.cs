using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHedgeState : PlayerSkillState
{
    public PlayerHedgeState(PlayerController player, PlayerStateMachine stateMachine, PlayerData playerData, string currentAnimation) : base(player, stateMachine, playerData, currentAnimation)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.RB.useGravity = false;
        Physics.IgnoreLayerCollision(9, 10, true);
    }

    public override void Exit()
    {
        base.Exit();
        player.RB.useGravity = true;
        Physics.IgnoreLayerCollision(9, 10, false);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Vector2 direction = axis;
        player.SetTotalVelocity(playerData.hedgeMovementVelocity, direction.normalized);
        player.FlipCheck(axis.x);
        Collider[] hedgeDetection = Physics.OverlapSphere(player.transform.position, playerData.hedgeDetectionRadius, playerData.hedge);
        if(hedgeDetection.Length == 0)
        {
            abilityDone = true;
        }
    }


    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
