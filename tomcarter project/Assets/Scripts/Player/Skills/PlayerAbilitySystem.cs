using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAbilitySystem : LoadableObject
{
    public event EventHandler<AbiltyUnlockedEventArgs> OnAbilityUnlocked;
    public class AbiltyUnlockedEventArgs : EventArgs {
        public List<Type> added;
        public List<Type> removed;
    }
    public enum PlayerSkill {
        SPORE_DASH,
        ROOT_ATTACK,
        VINE_HOOK,
        BARK_GUARD,
        HEDGE_CLIMB
    }

    public List<Type> GetAvailableStates()
    {
        Debug.Log("Get available states");
        var states = new List<Type>();

        states.Add(typeof(PlayerIdleState));
        states.Add(typeof(PlayerMovementState));
        states.Add(typeof(PlayerLandState));
        states.Add(typeof(PlayerOnAirState));
        states.Add(typeof(PlayerJumpState));
        states.Add(typeof(PlayerRootsState));
        states.Add(typeof(PlayerMeleeState));
        states.Add(typeof(PlayerDashJumpState));
        states.Add(typeof(PlayerDamagedState));
        states.Add(typeof(PlayerDialogueState));

        if (unlockedSkills[PlayerSkill.SPORE_DASH]) 
        {
            states.Add(typeof(PlayerBlinkDashState));
        }
        else
        {
            states.Add(typeof(PlayerBaseDashState));
        }
        if (unlockedSkills[PlayerSkill.ROOT_ATTACK])
        {
            states.Add(typeof(PlayerRangeState));
        }
        if (unlockedSkills[PlayerSkill.VINE_HOOK])
        {
            states.Add(typeof(PlayerHookState));
        }
        if (unlockedSkills[PlayerSkill.BARK_GUARD])
        {
            states.Add(typeof(PlayerHardenState));
        }
        if (unlockedSkills[PlayerSkill.HEDGE_CLIMB])
        {
            states.Add(typeof(PlayerHedgeState));
        }

        return states;
    }

    public void UnlockAbility(PlayerSkill skill)
    {
        if (unlockedSkills[skill]) return;
        
        unlockedSkills[skill] = true;
        var eventArgs = new AbiltyUnlockedEventArgs();
        eventArgs.removed = new List<Type>();
        switch (skill) 
        {
            case PlayerSkill.SPORE_DASH:
                eventArgs.added = new List<Type>() { typeof(PlayerBlinkDashState) };
                eventArgs.removed = new List<Type>() { typeof(PlayerBaseDashState) };
                break;
            case PlayerSkill.ROOT_ATTACK:
                eventArgs.added = new List<Type>() { typeof(PlayerRangeState) };
                break;
            case PlayerSkill.VINE_HOOK:
                eventArgs.added = new List<Type>() { typeof(PlayerHookState) };
                break;
            case PlayerSkill.BARK_GUARD:
                eventArgs.added = new List<Type>() { typeof(PlayerHardenState) };
                break;
            case PlayerSkill.HEDGE_CLIMB:
                eventArgs.added = new List<Type>() { typeof(PlayerHedgeState) };
                break;
        }
        OnAbilityUnlocked?.Invoke(this, eventArgs);
    }
    private Dictionary<PlayerSkill, bool> unlockedSkills = new Dictionary<PlayerSkill, bool>() 
    {
        { PlayerSkill.SPORE_DASH, false },
        { PlayerSkill.ROOT_ATTACK, false },
        { PlayerSkill.VINE_HOOK, false },
        { PlayerSkill.BARK_GUARD, false },
        { PlayerSkill.HEDGE_CLIMB, false }
    };
    protected override void LoadFromSavedGameState(GameState gameState)
    {
        Debug.Log("Loaded save data for Unlocked abilities");
        var unlockedAbilities = gameState.unlockedAbilities;
        unlockedSkills[PlayerSkill.SPORE_DASH] = unlockedAbilities._hasSporeDashUnlocked;
        unlockedSkills[PlayerSkill.ROOT_ATTACK] = unlockedAbilities._hasRootAttackUnlocked;
        unlockedSkills[PlayerSkill.VINE_HOOK] = unlockedAbilities._hasVineHookUnlocked;
        unlockedSkills[PlayerSkill.BARK_GUARD] = unlockedAbilities._hasBarkGuardUnlocked;
    }
    protected override void SaveToGameState(object sender, SaveLoadController.OnSaveCalledEventArgs onSaveArguments) 
    {
        var unlockedAbilitiesData =  new UnlockedAbilitiesData();
        unlockedAbilitiesData._hasSporeDashUnlocked = unlockedSkills[PlayerSkill.SPORE_DASH];
        unlockedAbilitiesData._hasRootAttackUnlocked = unlockedSkills[PlayerSkill.ROOT_ATTACK];
        unlockedAbilitiesData._hasVineHookUnlocked = unlockedSkills[PlayerSkill.VINE_HOOK];
        unlockedAbilitiesData._hasBarkGuardUnlocked = unlockedSkills[PlayerSkill.BARK_GUARD];
        onSaveArguments.gameState.unlockedAbilities = unlockedAbilitiesData;
    }


}
