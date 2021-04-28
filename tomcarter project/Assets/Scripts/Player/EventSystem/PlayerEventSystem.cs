using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerEventSystem 
{
    public class OnLandEventArgs : EventArgs {
        public Vector3 landingSpot;
    } 
    public event EventHandler<EventArgs> OnDamageTaken;
    public event EventHandler<OnLandEventArgs> OnGroundLand;
    public event EventHandler<EventArgs> OnHedgeEnter;
    private static PlayerEventSystem _instance;

    private PlayerEventSystem() {}

    public static PlayerEventSystem GetInstance() 
    {
        if (_instance == null)
        {
            _instance = new PlayerEventSystem();
        }
        return _instance;
    }

    public void TriggerPlayerHasTakenDamage() 
    {
        OnDamageTaken?.Invoke(null, EventArgs.Empty);
    }
    public void TriggerPlayerHasLanded(Vector3 landingSpot) 
    {
        OnGroundLand?.Invoke(null, new OnLandEventArgs() { landingSpot = landingSpot });
    }
    public void TriggerPlayerEnteredHedge() 
    {
        OnHedgeEnter?.Invoke(null, new EventArgs());
    }
}
