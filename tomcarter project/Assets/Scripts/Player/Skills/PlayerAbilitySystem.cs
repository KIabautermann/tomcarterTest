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
        { PlayerSkill.SPORE_DASH, true },
        { PlayerSkill.ROOT_ATTACK, true },
        { PlayerSkill.VINE_HOOK, true },
        { PlayerSkill.BARK_GUARD, true },
        { PlayerSkill.HEDGE_CLIMB, true },
    };
    public ComponentCache<PlayerState> GetAvailableStates()
    {
        var activeStates = new List<PlayerState>();
        var inactiveStates = new List<PlayerState>();
        var stateListMap = new Dictionary<bool, List<PlayerState>>() {
            { true, activeStates},
            { false, inactiveStates}
        };
        
        activeStates.Add(this.gameObject.GetComponent<PlayerIdleState>());
        activeStates.Add(this.gameObject.GetComponent<PlayerMovementState>());
        activeStates.Add(this.gameObject.GetComponent<PlayerLandState>());
        activeStates.Add(this.gameObject.GetComponent<PlayerOnAirState>());
        activeStates.Add(this.gameObject.GetComponent<PlayerJumpState>());
        activeStates.Add(this.gameObject.GetComponent<PlayerRootsState>());
        activeStates.Add(this.gameObject.GetComponent<PlayerMeleeState>());
        activeStates.Add(this.gameObject.GetComponent<PlayerDashJumpState>());
        activeStates.Add(this.gameObject.GetComponent<PlayerDamagedState>());
        activeStates.Add(this.gameObject.GetComponent<PlayerDialogueState>());

        stateListMap[unlockedSkills[PlayerSkill.SPORE_DASH]].Add(this.gameObject.GetComponent<PlayerBlinkDashState>());
        stateListMap[!unlockedSkills[PlayerSkill.SPORE_DASH]].Add(this.gameObject.GetComponent<PlayerBaseDashState>());
        stateListMap[unlockedSkills[PlayerSkill.ROOT_ATTACK]].Add(this.gameObject.GetComponent<PlayerRangeState>());
        stateListMap[unlockedSkills[PlayerSkill.ROOT_ATTACK]].Add(this.gameObject.GetComponent<PlayerBounceJumpState>());
        stateListMap[unlockedSkills[PlayerSkill.VINE_HOOK]].Add(this.gameObject.GetComponent<PlayerHookState>());
        stateListMap[unlockedSkills[PlayerSkill.BARK_GUARD]].Add(this.gameObject.GetComponent<PlayerHardenState>());
        stateListMap[unlockedSkills[PlayerSkill.HEDGE_CLIMB]].Add(this.gameObject.GetComponent<PlayerHedgeState>());
       
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
        unlockedSkills[PlayerSkill.SPORE_DASH] = false;
        unlockedSkills[PlayerSkill.ROOT_ATTACK] = false;
        unlockedSkills[PlayerSkill.VINE_HOOK] = false;
        unlockedSkills[PlayerSkill.BARK_GUARD] = false;
        unlockedSkills[PlayerSkill.HEDGE_CLIMB] = true;
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
