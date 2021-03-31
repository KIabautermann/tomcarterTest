using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    public bool TakingDamage { get; private set; }
    private float _lastTimeHit = 0;

    [SerializeField]
    public float invulnerabilityPeriod = 1f;
    
    private void Start() {
        // Subscribe to some damage event

        // Something += PlayerHealth_OnDamageTaken;
    }
    public void PlayerHealth_OnDamageTaken(object sender, EventArgs eventArgs) 
    {
        TakingDamage = true;
        _lastTimeHit = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // Damage Condition
        if (Time.time > _lastTimeHit + invulnerabilityPeriod) 
        {
            TakingDamage = false;
        }
    }
}
