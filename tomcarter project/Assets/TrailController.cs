using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailController : MonoBehaviour
{
    private Rigidbody _targetRB;
    [SerializeField]
    private Rigidbody _trailRB;
    private bool locked;
    public float distance;
    public float minSpeed;
    public float maxSpeed;
    public float accelerationTime;
    private float _currentMaxSpeed;
    private float speed;
    private Vector3 velocity;
    Vector3 direction;
    Vector3 target;
    void Start()
    {
        locked = true;
        _targetRB = GetComponent<Rigidbody>();
    }

    void Update()
    {     
        if(!locked){
            if(_targetRB.velocity != Vector3.zero){
                target = _targetRB.position - _targetRB.velocity.normalized * distance;
            }
            else{
                target = _targetRB.position;
            }
            direction = (target - _trailRB.transform.position).normalized; 
            speed = Mathf.Clamp(_targetRB.velocity.magnitude , minSpeed, _currentMaxSpeed);    
            if(_currentMaxSpeed < maxSpeed){
                _currentMaxSpeed+= maxSpeed * Time.deltaTime/accelerationTime;
            }
            else{
                _currentMaxSpeed = maxSpeed;
            }
            if(Vector3.Distance(_trailRB.position,_targetRB.position)>distance*3){
                speed = maxSpeed;
            }
        }
        else{
            _trailRB.transform.position=_targetRB.position;
        }
    }
    private void FixedUpdate() {
        if(!locked){
            _trailRB.velocity = direction * speed;
        }
    }
    public void Lock (){
        locked = true;
        _currentMaxSpeed=minSpeed;
    }
    public void Unlock (){
        locked = false;      
    }
}
