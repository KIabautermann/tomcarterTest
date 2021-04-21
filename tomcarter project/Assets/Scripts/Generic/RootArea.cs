using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootArea : MonoBehaviour
{
    [SerializeField]
    private PlayerAbilitySystem.PlayerSkill unlockableSkill;
    public PlayerAbilitySystem.PlayerSkill GetSkill()
    {
        return unlockableSkill;
    }
}
