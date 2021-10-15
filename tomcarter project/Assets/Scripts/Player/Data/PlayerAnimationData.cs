using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerAnimationData", menuName = "Data/PlayerData/PlayerAnimationData")]
public class PlayerAnimationData : ScriptableObject 
{
    public AnimationClip airDownwards;
    public AnimationClip airJump;
    public AnimationClip airLand;
    public AnimationClip airPeak;
    public AnimationClip airUpwards;
    public AnimationClip attackAir;
    public AnimationClip attackGround;
    public AnimationClip attackRange;
    public AnimationClip attackRangeUp;
    public AnimationClip attackRangeDown;
    public AnimationClip blinkEnd;
    public AnimationClip blinkInit;
    public AnimationClip crouch;
    public AnimationClip crouchInit;
    public AnimationClip dash;
    public AnimationClip dashEnd;
    public AnimationClip harden;
    public AnimationClip hardenEnd;
    public AnimationClip hardenInit;
    public AnimationClip hedgeInit;
    public AnimationClip hedge;
    public AnimationClip hookBlur;
    public AnimationClip hookMotion;
    public AnimationClip hookThrow;
    public AnimationClip idle;
    public AnimationClip rootsAbsorb;
    public AnimationClip rootsEnd;
    public AnimationClip rootsInit;
    public AnimationClip run;
    public AnimationClip runInit;   
}
