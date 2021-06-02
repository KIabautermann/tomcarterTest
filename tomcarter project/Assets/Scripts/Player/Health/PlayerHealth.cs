using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    public bool TakingDamage { get; private set; }
    private float _lastTimeHit = 0;

    [SerializeField]
    public float _invulnerabilityPeriod = 1f;
    public PlayerData data;
    

    private void OnCollisionEnter(Collision other) 
    {    
        int collidedLayer = other.gameObject.layer;
        if ((data.damage.value & 1 << collidedLayer) != 0 || (data.damage.value & 1 << collidedLayer) != 0)
        {
            TakingDamage = true;
            _lastTimeHit = Time.time;   
            PlayerEventSystem.GetInstance().TriggerPlayerHasTakenDamage();
        }
        
        if (data.hazard == (data.hazard | (1 << collidedLayer)))
        {
            PlayerEventSystem.GetInstance().TriggerPlayerCollidedHazard();
        }
    }

}
