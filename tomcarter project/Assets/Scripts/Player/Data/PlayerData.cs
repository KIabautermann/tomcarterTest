using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "Data/PlayerData/PlayerBaseData")]
public class PlayerData : ScriptableObject
{
    [Header("General")]
    public float collisionDetection = .1f;
    [Header("Move State")]
    public string idleTrigger = "idle";
    public string movementTrigger = "run";
    public float movementVelocity = 10;
    public float groundedAccelerationTime = .2f;
    public float airAccelerationTime = .4f;

    [Header("Jump State")]
    public string airTrigger = "air";
    public string jumpTrigger = "jump";
    public string landTrigger = "land";
    public float jumpVelocity = 15;
    public float jumpLenght = 1;
    public float fallMultiplier = 2;
    public float minJumpVelocity = 1;
    public float coyoteTime = .2f;
    public float shortHopMultiplier = .5f;
    public float maxFallVelocity = -20;
    public int amountOfJumps = 1;

    [Header("Dash State")]
    public string dashTrigger = "dash";
    public string blinkTrigger = "blink";
    public float dashCooldown = .3f;
    public float dashStartUp = .05f;
    public float dashLenght = .4f;
    public float blinkDashLenght = .2f;  
    public float dashSpeed = 15f;
    public float blinkDashSpeed = 30f;
    public float drag = 10f;
    public float dashEndMultiplier= .7f;

    [Header("DashJump State")]
    public string dashJumpTrigger = "dash_jump";
    public float dashJumpVelocityX = 12;
    public float dashJumpAccelerationTime = 1f;
    public float dashJumpFallMultiplier = 2;
    public float minDashJumpVelocity = 1;
    public float dashCoyoteTime=.3f;
    public float jumpHandicapTime = .2f;

    [Header("Hook State")]
    public string hookTrigger = "hook";
    public float hookSpeed = 15;
    public float circularSpeed = 5;
    public float hookDetectionRadius = .1f;
    public float maxAngle = 45;
    public float hookCooldown = .5f;
    public float yVelocityMultiplier = .8f;
    public float hookAimAssistConeAngle = 20f;
    public Vector3 hookTarget;
    public float hookMaxDistance;

    [Header("Hedge State")]

    public string hedgeTrigger = "hedge";
    public float hedgeDetectionOffset;
    public float hedgeTransitionInPush;
    public float hedgeTransitionOutPush;
    public float hedgeTransitionInTime;
    public float hedgeJumpHandicapTime = 0.4f;
  
    [Header("Dialogue State")]
    public string dialogueTrigger = "talk";
    public float npcDetectionLenght = 10f;
    public float npcDetectionRadius = .25f;
    
    [Header("Root State")]
    public float rootChannelingDuration = 2f;
    public string rootTrigger = "root";

    [Header("Damaged State")]
    public string damageTrigger = "damage";
    public float damagedDuration = 1f;

    [Header("Harden State")]
    public string hardenTrigger = "harden";
    public float hardenTime = 2f;
    public float hardenMovementSpeed = 5f;
    public float hardenHookMultiplier = 1.2f;
    public float hardenDashVelocity = 20f;
    public float hardenDashChannelingTime = 1f;
    public float minBreakVelocity = 10;
    public float ceilingExceptionMultiplier = 1.7f;

    [Header("Melee State")]

    public string meleeTrigger = "melee";
    public float meleeStartupTime = .5f;
    public float meleeHitboxTime = .5f;
    public float meleerecoveryTime = .5f;
    public Vector2 meleeHitbox = Vector2.one;
    public Vector2 meleeHiboxOffset;


    [Header("Range State")]

    public string rangeTrigger = "range";
    public string longRangeTrigger = "long_range";
    public string chargeTrigger = "charge";
    public float rangeStartupTime = .5f;
    public float rangeHitboxTime = .5f;
    public float rangerecoveryTime = .5f;
    public float rangeCooldown;
    public float rangeChargeTime;
    public Vector2 rangeHitbox = Vector2.one;
    public Vector2 rangeHiboxOffset;
    public Vector2 chargedRangeHitbox = Vector2.one;
    public Vector2 chargedRangeHiboxOffset;
    public float rangeInitialImpulse;

    [Header("LayerMasks")]
    public LayerMask walkable;
    public LayerMask hedge;
    public LayerMask npc;
    public LayerMask rootable;
    public LayerMask solidGround;
    public LayerMask hazard;
    public LayerMask damage;

}
