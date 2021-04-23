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
        if (inputs.RootsCancel) 
        {
            stateDone = true;
        }
        else if (Time.time > startTime + stats.rootChannelingDuration)
        {
            _channelFinished = true;
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
        controller.SetAcceleration(0f);
        controller.SetVelocityX(0f);
    
        Component rootArea = controller.GetRootableHit().transform.gameObject.GetComponent<RootArea>();
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
            abilitySystem.UnlockAbility(rootAreaComponent.GetSkill());
        }
        base.DoTransitionOut();
    }

    protected override void TransitionChecks()
    {
        if (_channelFinished) 
        {
            stateDone = true;
        }

        base.TransitionChecks();
    }

    public override void Init(PlayerStateMachine target)
    {
        abilitySystem = GetComponent<PlayerAbilitySystem>();
        base.Init(target);
    }
}
