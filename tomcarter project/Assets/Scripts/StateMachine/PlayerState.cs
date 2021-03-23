using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : State<PlayerStateMachine>
{  
    protected PlayerInputHandler inputs;
    protected PlayerData stats;
    private void Start() 
    {
        inputs = GetComponent<PlayerInputHandler>();
        stats = GetComponent<PlayerData>();
    }
}
