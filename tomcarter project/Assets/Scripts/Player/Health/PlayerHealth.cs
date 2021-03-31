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
    
    private void PlayerHealth_OnDamageTaken() 
    {
        TakingDamage = true;
        _lastTimeHit = Time.time;
    }

    private void OnCollisionEnter(Collision other) 
    {    
        if (other.gameObject.layer == Math.Log(_damageLayer.value, 2))
        {
            PlayerHealth_OnDamageTaken();
        }
    }

    void Update()
    {
        if (Time.time > _lastTimeHit + _invulnerabilityPeriod) 
        {
            TakingDamage = false;
        }
    }
}
