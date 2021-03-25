using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AbiltySystem : LoadableObject
{
    public enum PlayerSkill {
        SPORE_DASH,
        ROOT_ATTACK,
        VINE_HOOK,
        BARK_GUARD
    }

    private Dictionary<PlayerSkill, bool> unlockedSkills = new Dictionary<PlayerSkill, bool>() 
    {
        { PlayerSkill.SPORE_DASH, false },
        { PlayerSkill.ROOT_ATTACK, false },
        { PlayerSkill.VINE_HOOK, false },
        { PlayerSkill.BARK_GUARD, false }
    };

    public bool IsUnlocked(PlayerSkill skill)
    {
        return unlockedSkills[skill];
    }

    public void UnlockSkill(PlayerSkill skill) 
    {
        Debug.Log("Unlocking Skill: " + skill.ToString());
        unlockedSkills[skill] = true;
    }
    protected override void LoadFromSavedGameState(GameState gameState)
    {
        ReadFromSerializedAbilities(gameState.unlockedAbilities);
    }
    protected override void SaveToGameState(object sender, SaveLoad.OnSaveCalledEventArgs onSaveArguments) 
    {
        onSaveArguments.gameState.unlockedAbilities = new UnlockedAbilitiesData(this);
    }

    private void ReadFromSerializedAbilities(UnlockedAbilitiesData unlockedAbilities) 
    {
        unlockedSkills[PlayerSkill.SPORE_DASH] = unlockedAbilities._hasSporeDashUnlocked;
        unlockedSkills[PlayerSkill.ROOT_ATTACK] = unlockedAbilities._hasRootAttackUnlocked;
        unlockedSkills[PlayerSkill.VINE_HOOK] = unlockedAbilities._hasVineHookUnlocked;
        unlockedSkills[PlayerSkill.BARK_GUARD] = unlockedAbilities._hasBarkGuardUnlocked;
    }
    
}
