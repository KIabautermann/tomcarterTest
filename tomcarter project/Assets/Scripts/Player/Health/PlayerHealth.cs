using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    [Range(0, 5)]
    public int currentHealth;
    private float _lastTimeHit = 0;
    [SerializeField]
    private CanvasReference _canvas;
    public Animator healthMeter;

    [SerializeField]
    public float _invulnerabilityPeriod = 1f;
    public PlayerData data;

    private void Start()
    {
        StartCoroutine(LoadScriptableObjects());
    }
    private void Update()
    {
        if(healthMeter != null) healthMeter.SetInteger("Health", currentHealth);
    }


    /*private void OnCollisionEnter(Collision other) 
    {    
        int collidedLayer = other.gameObject.layer;

        if (_lastTimeHit + _invulnerabilityPeriod > Time.time) return;

        if ((data.damage.value & 1 << collidedLayer) != 0)
        {
            _lastTimeHit = Time.time;   
            PlayerEventSystem.GetInstance().TriggerPlayerHasTakenDamage();
        }
    }*/

    public void TakeDamage(){
        if (_lastTimeHit + _invulnerabilityPeriod > Time.time) return;

        else{
            _lastTimeHit = Time.time;   
            PlayerEventSystem.GetInstance().TriggerPlayerHasTakenDamage();
        }
    }
    
    private IEnumerator LoadScriptableObjects() 
    {
        while (!_canvas.IsLoaded) {
            yield return new WaitForEndOfFrame();
        }
        healthMeter = _canvas.GetImageForGameObject(CanvasElement.HealthMeter).GetComponent<Animator>();
    }


}
