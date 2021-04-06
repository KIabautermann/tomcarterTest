using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "Data/PlayerData/PlayerBaseData")]
public class PlayerData : ScriptableObject
{
     [Header("Move State")]
    public float movementVelocity = 10;
    public float groundedAccelerationTime = .2f;
    public float airAccelerationTime = .4f;

    [Header("Jump State")]
    public float jumpVelocity = 15;
    public float fallMultiplier = 2;
    public float minJumpVelocity = 1;
    public float coyoteTime = .2f;
    public float shortHopMultiplier = .5f;
    public float maxFallVelocity = -20;
    public int amountOfJumps = 1;

    [Header("Dash State")]
    public float dashCooldown = .3f;
    public float dashLenght = .4f;
    public float blinkDashLenght = .2f;  
    public float dashSpeed = 15f;
    public float blinkDashSpeed = 30f;
    public float drag = 10f;
    public float dashEndMultiplier= .7f;
    public float collisionDetection= 1f;

    [Header("DashJump State")]
    public float dashJumpVelocityX = 12;
    public float dashJumpAccelerationTime = 1f;
    public float dashJumpFallMultiplier = 2;
    public float minDashJumpVelocity = 1;
    public float dashCoyoteTime=.3f;
    public float jumpHandicapTime = .2f;

    [Header("Hook State")]
    public float hookSpeed = 15;
    public float circularSpeed = 5;
    public float hookDetectionRadius = .1f;
    public float maxAngle = 45;
    public float hookCooldown = .5f;
    public Vector3 hookTarget;

    [Header("Hedge State")]
    public float hedgeDetectionLenght = .1f;
    public float hedgeDetectionRadius = .25f;
    
    [Header("Dialogue State")]
    public float npcDetectionLenght = 10f;
    public float npcDetectionRadius = .25f;
    public LayerMask npc;
    
    [Header("Root State")]
    public float rootChannelingDuration = 2f;

    [Header("Damaged State")]
    public float damagedDuration = 1f;

    [Header("LayerMasks")]
    public LayerMask walkable;
    public LayerMask hedge;

}
