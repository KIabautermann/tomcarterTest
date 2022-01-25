using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    public bool TakingDamage { get; private set; }
    [Range(0, 5)]
    public int currentHealth;
    private float _lastTimeHit = 0;
    public Animator healthMeter;

    [SerializeField]
    public float _invulnerabilityPeriod = 1f;
    public PlayerData data;

    private void Start()
    {
        if(GameObject.Find("Health Meter")!=null) healthMeter = GameObject.Find("Health Meter").GetComponent<Animator>();
    }

    private void Update()
    {
        if(healthMeter!=null)healthMeter.SetInteger("Health", currentHealth);
        else healthMeter = GameObject.Find("Health Meter").GetComponent<Animator>();
    }


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
