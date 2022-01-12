using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewVFXRepository", menuName = "Data/VFX Repository")]
public class VisualEffectsRepository : ScriptableObject
{
    public AnimationClip afterimage;
    public AnimationClip airBlast;
    public AnimationClip sporeBlast;
    public AnimationClip playerGroundAttack;
    public AnimationClip playerAirAttack;
    public AnimationClip playerVineIdle;
    public AnimationClip playerVineEnd;
}
