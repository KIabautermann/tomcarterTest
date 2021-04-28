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

    [SerializeField]
    private LayerMask _damageLayer;
    

    private void OnCollisionEnter(Collision other) 
    {    
        if (other.gameObject.layer == Math.Log(_damageLayer.value, 2))
        {
            TakingDamage = true;
            _lastTimeHit = Time.time;   
            PlayerEventSystem.GetInstance().TriggerPlayerHasTakenDamage();
        }
    }

}
