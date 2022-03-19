using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootArea : MonoBehaviour
{
    [SerializeField]
    private bool isPermanent;
    [SerializeField]
    private PlayerSkill unlockableSkill;
    public PlayerSkill GetSkill() => unlockableSkill;
    public bool IsPermanentUnlock() => isPermanent;
}
