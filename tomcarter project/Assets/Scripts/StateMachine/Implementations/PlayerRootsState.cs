using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRootsState : PlayerSkillState
{
    
    protected PlayerAbilitySystem abilitySystem;
    private RootArea rootAreaComponent;
    private bool _channelFinished = false;
    protected override void DoChecks()
    {
         base.DoChecks();
    }

    protected override void DoLogicUpdate()
    {
        if (inputs.RootsCancel && !_channelFinished) 
        {
            stateDone = true;
        }
        else if (counter > stats.rootChannelingDuration)
        {
            _target.QueueAnimation(_target.animations.rootsEnd.name, true, false);
            _channelFinished = true;
            stateDone = true;
        }
        base.DoLogicUpdate();
    }

    protected override void DoPhysicsUpdate()
    {
        base.DoPhysicsUpdate();
    }

    protected override void DoTransitionIn()
    {
        _channelFinished = false;
        _target.QueueAnimation(_target.animations.rootsInit.name, false, true);
        controller.SetAcceleration(0f);
        controller.SetVelocityX(0f);
    
        if (!controller.OnRootable()) 
        {
            Debug.LogWarning("Entered rootable, but already left after transition start");
            stateDone = true;
            return;
        }
        
        Component rootArea = controller.GetRootableHit().collider.gameObject.GetComponent<RootArea>();
        if (rootArea == null) 
        {
            Debug.LogWarning("Root Layer game object does not have an RootArea component. Exiting state");
            stateDone = true;   
        } 
        else
        {
            rootAreaComponent = rootArea as RootArea;
        }

        base.DoTransitionIn();
    }

    protected override void DoTransitionOut()
    {
        if (_channelFinished) {
            if (rootAreaComponent.IsPermanentUnlock()) {
                abilitySystem.PermanentlyUnlockAbility(rootAreaComponent.GetSkill());
            } else {
                abilitySystem.ToggleLockAbility(rootAreaComponent.GetSkill(), true);
            }
        }
        _target.QueueAnimation(_target.animations.rootsEnd.name, true, true);
        base.DoTransitionOut();
    }

    protected override void TransitionChecks()
    {
        if (_channelFinished) 
        {
            animationIndex = 2;
            if(endByAnimation){
                stateDone = true;
            }
        }

        base.TransitionChecks();
    }

    public override void Init(PlayerStateMachine target)
    {
        abilitySystem = GetComponent<PlayerAbilitySystem>();
        base.Init(target);
        animationTrigger = stats.rootID;
        stateIndex = stats.rootsNumberID;
    }
}
