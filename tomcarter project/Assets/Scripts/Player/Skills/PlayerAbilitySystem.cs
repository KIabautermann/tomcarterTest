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
        HARD_LAND,
        HEDGE_CLIMB
    }

    public ComponentCache<PlayerState> GetAvailableStates()
    {
        Debug.Log("Get available states");
        var activeStates = new List<PlayerState>();
        var inactiveStates = new List<PlayerState>();
        var stateListMap = new Dictionary<bool, List<PlayerState>>() {
            { true, activeStates},
            { false, inactiveStates}
        };
        
        activeStates.Add(this.gameObject.AddComponent<PlayerIdleState>());
        activeStates.Add(this.gameObject.AddComponent<PlayerMovementState>());
        activeStates.Add(this.gameObject.AddComponent<PlayerLandState>());
        activeStates.Add(this.gameObject.AddComponent<PlayerOnAirState>());
        activeStates.Add(this.gameObject.AddComponent<PlayerJumpState>());
        activeStates.Add(this.gameObject.AddComponent<PlayerRootsState>());
        activeStates.Add(this.gameObject.AddComponent<PlayerMeleeState>());
        activeStates.Add(this.gameObject.AddComponent<PlayerDashJumpState>());
        activeStates.Add(this.gameObject.AddComponent<PlayerDamagedState>());
        activeStates.Add(this.gameObject.AddComponent<PlayerDialogueState>());
        activeStates.Add(this.gameObject.AddComponent<PlayerHardenLandState>());

        stateListMap[unlockedSkills[PlayerSkill.SPORE_DASH]].Add(this.gameObject.AddComponent<PlayerBlinkDashState>());
        stateListMap[!unlockedSkills[PlayerSkill.SPORE_DASH]].Add(this.gameObject.AddComponent<PlayerBaseDashState>());
        stateListMap[unlockedSkills[PlayerSkill.ROOT_ATTACK]].Add(this.gameObject.AddComponent<PlayerRangeState>());
        stateListMap[unlockedSkills[PlayerSkill.VINE_HOOK]].Add(this.gameObject.AddComponent<PlayerHookState>());
        stateListMap[unlockedSkills[PlayerSkill.BARK_GUARD]].Add(this.gameObject.AddComponent<PlayerHardenState>());
        stateListMap[unlockedSkills[PlayerSkill.HEDGE_CLIMB]].Add(this.gameObject.AddComponent<PlayerHedgeState>());
        stateListMap[unlockedSkills[PlayerSkill.HARD_LAND]].Add(this.gameObject.AddComponent<PlayerHardenLandState>());
       
        return new ComponentCache<PlayerState>(stateListMap[true], stateListMap[false]);
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
            case PlayerSkill.HARD_LAND:
                eventArgs.added= new List<Type>() {typeof(PlayerHardenLandState)};
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
        { PlayerSkill.HEDGE_CLIMB, false },
        { PlayerSkill.HARD_LAND, false }
    };
    protected override void LoadFromSavedGameState(GameState gameState)
    {
        var unlockedAbilities = gameState.unlockedAbilities;
        unlockedSkills[PlayerSkill.SPORE_DASH] = unlockedAbilities._hasSporeDashUnlocked;
        unlockedSkills[PlayerSkill.ROOT_ATTACK] = unlockedAbilities._hasRootAttackUnlocked;
        unlockedSkills[PlayerSkill.VINE_HOOK] = unlockedAbilities._hasVineHookUnlocked;
        unlockedSkills[PlayerSkill.BARK_GUARD] = unlockedAbilities._hasBarkGuardUnlocked;
        unlockedSkills[PlayerSkill.HEDGE_CLIMB] = unlockedAbilities._hasHedgeClimbUnlocked;
        unlockedSkills[PlayerSkill.HARD_LAND] = unlockedAbilities._hasBarkGuardUnlocked;
    }
    protected override void SaveToGameState(object sender, SaveLoadController.OnSaveCalledEventArgs onSaveArguments) 
    {
        var unlockedAbilitiesData =  new UnlockedAbilitiesData();
        unlockedAbilitiesData._hasSporeDashUnlocked = unlockedSkills[PlayerSkill.SPORE_DASH];
        unlockedAbilitiesData._hasRootAttackUnlocked = unlockedSkills[PlayerSkill.ROOT_ATTACK];
        unlockedAbilitiesData._hasVineHookUnlocked = unlockedSkills[PlayerSkill.VINE_HOOK];
        unlockedAbilitiesData._hasBarkGuardUnlocked = unlockedSkills[PlayerSkill.BARK_GUARD];
        unlockedAbilitiesData._hasHedgeClimbUnlocked = unlockedSkills[PlayerSkill.HEDGE_CLIMB];
        onSaveArguments.gameState.unlockedAbilities = unlockedAbilitiesData;
    }


}
