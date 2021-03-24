using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTestStateB : PlayerState
{
    public override bool Equals(object other)
    {
        return base.Equals(other);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return base.ToString();
    }

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
        Debug.Log("State B");
    }

    protected override void DoTransitionOut()
    {
        
    }

    protected override void TransitionChecks()
    {
        if(inputs.JumpInput)
        {
            _target.ChangeState<PlayerTestStateA>();
            inputs.UsedJump();
        }
    }
}
