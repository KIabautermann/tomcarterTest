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
    private Dictionary<PlayerSkill, bool> unlockedSkills = new Dictionary<PlayerSkill, bool>() 
    {
        { PlayerSkill.SPORE_DASH, false },
        { PlayerSkill.ROOT_ATTACK, false },
        { PlayerSkill.VINE_HOOK, false },
        { PlayerSkill.BARK_GUARD, false },
        { PlayerSkill.HEDGE_CLIMB, false },
    };
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

        stateListMap[unlockedSkills[PlayerSkill.SPORE_DASH]].Add(this.gameObject.AddComponent<PlayerBlinkDashState>());
        stateListMap[!unlockedSkills[PlayerSkill.SPORE_DASH]].Add(this.gameObject.AddComponent<PlayerBaseDashState>());
        stateListMap[unlockedSkills[PlayerSkill.ROOT_ATTACK]].Add(this.gameObject.AddComponent<PlayerRangeState>());
        stateListMap[unlockedSkills[PlayerSkill.ROOT_ATTACK]].Add(this.gameObject.AddComponent<PlayerChargedRangeState>());
        stateListMap[unlockedSkills[PlayerSkill.ROOT_ATTACK]].Add(this.gameObject.AddComponent<PlayerRangeChargeState>());
        stateListMap[unlockedSkills[PlayerSkill.VINE_HOOK]].Add(this.gameObject.AddComponent<PlayerHookState>());
        stateListMap[unlockedSkills[PlayerSkill.BARK_GUARD]].Add(this.gameObject.AddComponent<PlayerHardenState>());
        stateListMap[unlockedSkills[PlayerSkill.HEDGE_CLIMB]].Add(this.gameObject.AddComponent<PlayerHedgeState>());
       
        return new ComponentCache<PlayerState>(stateListMap[true], stateListMap[false]);
    }

    public bool IsPermanentlyUnlocked(Type state) {
        if (!AbilitySystemConstants.StateSkillDictionary.TryGetValue(state, out PlayerSkill skill)) return true;
        return unlockedSkills[skill];
    }

    public int GetSkillUsesAmount(Type state) {
        if (!AbilitySystemConstants.StateSkillDictionary.TryGetValue(state, out PlayerSkill skill)) return int.MaxValue;
        return AbilitySystemConstants.SkillLimitedUses[skill];
    }
    public void ExpireSkillStateUses(Type state) {
        if (!AbilitySystemConstants.StateSkillDictionary.TryGetValue(state, out PlayerSkill skill)) return;
        ToggleLockAbility(skill, false);
    }

    public void PermanentlyUnlockAbility(PlayerSkill skill) {
        
        // La habilidad ya esta desbloqueada y se pide desbloquear de vuelta
        if (unlockedSkills[skill]) {
            Debug.LogWarning("Se esta intentando desbloquear permanentemente una habilidad ya desbloqueada");
            return;
        }
        ToggleLockAbility(skill, true);
        unlockedSkills[skill] = true;
    }
    public void ToggleLockAbility(PlayerSkill skill, bool unlock)
    {
        // La habilidad ya esta desbloqueada permanentemente
        if (unlockedSkills[skill]) return;

        List<Type> typesToSwap_1 = new List<Type>();
        List<Type> typesToSwap_2 = new List<Type>();
        switch (skill) 
        {
            case PlayerSkill.SPORE_DASH:
                typesToSwap_1 = new List<Type>() { typeof(PlayerBlinkDashState) };
                typesToSwap_2 = new List<Type>() { typeof(PlayerBaseDashState) };
                break;
            case PlayerSkill.ROOT_ATTACK:
                typesToSwap_1 = new List<Type>() { typeof(PlayerRangeState) };
                break;
            case PlayerSkill.VINE_HOOK:
                typesToSwap_1 = new List<Type>() { typeof(PlayerHookState) };
                break;
            case PlayerSkill.BARK_GUARD:
                typesToSwap_1 = new List<Type>() { typeof(PlayerHardenState) };
                break;
            case PlayerSkill.HEDGE_CLIMB:
                typesToSwap_1 = new List<Type>() { typeof(PlayerHedgeState) };
                break;
        }
        
        var eventArgs = new AbiltyUnlockedEventArgs();
        eventArgs.removed = !unlock ? typesToSwap_1 : typesToSwap_2;
        eventArgs.added = unlock ? typesToSwap_1 : typesToSwap_2;

        OnAbilityUnlocked?.Invoke(this, eventArgs);
    }
    
    protected override void LoadFromSavedGameState(GameState gameState)
    {
        var unlockedAbilities = gameState.unlockedAbilities;
        unlockedSkills[PlayerSkill.SPORE_DASH] = unlockedAbilities._hasSporeDashUnlocked;
        unlockedSkills[PlayerSkill.ROOT_ATTACK] = unlockedAbilities._hasRootAttackUnlocked;
        unlockedSkills[PlayerSkill.VINE_HOOK] = unlockedAbilities._hasVineHookUnlocked;
        unlockedSkills[PlayerSkill.BARK_GUARD] = unlockedAbilities._hasBarkGuardUnlocked;
        unlockedSkills[PlayerSkill.HEDGE_CLIMB] = unlockedAbilities._hasHedgeClimbUnlocked;
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
