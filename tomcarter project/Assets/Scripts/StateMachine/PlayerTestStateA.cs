using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTestStateA : PlayerState
{
  
    protected override void DoChecks()
    {
        
    }

    protected override void DoLogicUpdate()
    {
        TransitionChecks();
    }

    protected override void DoPhysicsUpdate()
    {
       
    }

    protected override void DoTransitionIn()
    {
        Debug.Log("State A");
    }

    protected override void DoTransitionOut()
    {
        
    }

    protected override void TransitionChecks()
    {
            
        if(inputs.JumpInput)
        {
            Debug.Log("Frame with jump input");
            inputs.UsedJump();
            // TODO: Borrar esto, esta de placeholder. No se que es lo que deberia triggerear un SaveFile
            if (!abiltySystem.IsUnlocked(AbiltySystem.PlayerSkill.SPORE_DASH)) 
            {
                Debug.Log("Cant change to state A since it's not unlocked. Let me unlock it for you");
                abiltySystem.UnlockSkill(AbiltySystem.PlayerSkill.SPORE_DASH); 
                SaveLoad saveLoad = FindObjectOfType<SaveLoad>();
                saveLoad.SaveGame();
                return;
            }

            _target.ChangeState<PlayerTestStateB>();
        }
    }

}
