using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AbilitySystemConstants
{
    public static readonly Dictionary<Type, PlayerSkill> StateSkillDictionary = new Dictionary<Type, PlayerSkill>() 
    {
        { typeof(PlayerHedgeState), PlayerSkill.HEDGE_CLIMB },
        { typeof(PlayerBlinkDashState), PlayerSkill.SPORE_DASH },
        { typeof(PlayerHardenState), PlayerSkill.BARK_GUARD },
        { typeof(PlayerRangeState), PlayerSkill.ROOT_ATTACK },
        { typeof(PlayerHookState), PlayerSkill.VINE_HOOK }
    };
    public static readonly Dictionary<PlayerSkill, int> SkillLimitedUses = new Dictionary<PlayerSkill, int>() 
    {
        { PlayerSkill.HEDGE_CLIMB, 3 },
        { PlayerSkill.ROOT_ATTACK, 3 },
        { PlayerSkill.BARK_GUARD, 3 },
        { PlayerSkill.VINE_HOOK, 3 },
        { PlayerSkill.SPORE_DASH, 3 }
    };
}
