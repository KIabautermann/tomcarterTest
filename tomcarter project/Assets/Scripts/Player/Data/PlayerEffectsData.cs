using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "Data/PlayerData/PlayerVFXData")]
public class PlayerEffectsData : ScriptableObject
{
   public AnimationClip dashAfterimage;
   public AnimationClip dashjumpUpAfterimage;
   public AnimationClip dashjumpDownAfterimage;
   public AnimationClip hardenAfterimage;
   public AnimationClip dashjumpWind;
   public AnimationClip sporeDashTrail;
}
