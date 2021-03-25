
[System.Serializable]
public class UnlockedAbilitiesData {
    public bool _hasSporeDashUnlocked = false;
    public bool _hasRootAttackUnlocked = false;
    public bool _hasVineHookUnlocked = false;
    public bool _hasBarkGuardUnlocked = false;

    public UnlockedAbilitiesData(AbiltySystem abilitySystem) 
    {
        _hasSporeDashUnlocked = abilitySystem.IsUnlocked(AbiltySystem.PlayerSkill.SPORE_DASH);
        _hasRootAttackUnlocked = abilitySystem.IsUnlocked(AbiltySystem.PlayerSkill.ROOT_ATTACK);
        _hasVineHookUnlocked = abilitySystem.IsUnlocked(AbiltySystem.PlayerSkill.VINE_HOOK);
        _hasBarkGuardUnlocked = abilitySystem.IsUnlocked(AbiltySystem.PlayerSkill.BARK_GUARD);
    }
    
    public UnlockedAbilitiesData() {}
}