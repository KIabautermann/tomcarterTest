using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNewStateMachine : MonoBehaviour
{
    void Start()
    {
        var allStates = GetComponents<PlayerBaseState<PlayerNewStateMachine>>();
    }

    void Update()
    {
        
    }
}
