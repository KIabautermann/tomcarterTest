using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilitySystem 
{
   public abstract List<State<PlayerStateMachine>> GetAvailableStates();
}
