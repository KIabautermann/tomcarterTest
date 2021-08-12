using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectAnimationRepository", menuName = "Effect Animation Repository")]
public class EffectAnimationRepository : ScriptableObject
{
    [SerializeField]
    public AnimationClip DashAfterImage;
    [SerializeField]
    public AnimationClip DashJumpAfterImage;
    [SerializeField]
    public AnimationClip DashJumpBlast;
    [SerializeField]
    public AnimationClip LandingBlast; 
}
