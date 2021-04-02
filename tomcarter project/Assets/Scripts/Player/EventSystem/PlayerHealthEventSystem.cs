using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHealthEventSystem 
{
    public event EventHandler<EventArgs> OnDamageTaken;
    private static PlayerHealthEventSystem _instance;

    private PlayerHealthEventSystem() {}

    public static PlayerHealthEventSystem GetInstance() 
    {
        if (_instance == null)
        {
            _instance = new PlayerHealthEventSystem();
        }
        return _instance;
    }

    public void TriggerPlayerHasTakenDamage() 
    {
        OnDamageTaken?.Invoke(null, EventArgs.Empty);
    }
}
