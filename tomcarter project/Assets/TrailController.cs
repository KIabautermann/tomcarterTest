using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailController : MonoBehaviour
{
    private Rigidbody _targetRB;
    [SerializeField]
    private Rigidbody _trailRB;
    [SerializeField]
    private GameObject inBetween;
    private SpriteRenderer inBetweenRenderer;
    private bool locked;
    public float distance;
    public float accelerationTime;
    private float speed;
    Vector3 direction;
    private Vector3 _target;
    private float speedMultiplier;
    private float aditionalMult;
    void Start()
    {
        locked = true;
        _targetRB = GetComponent<Rigidbody>();
        _trailRB.isKinematic = true; 
        inBetweenRenderer = inBetween.GetComponent<SpriteRenderer>();
    }

    void Update()
    {     
       if(!locked){
           _target = _targetRB.transform.position - (_targetRB.velocity.normalized * distance);
           direction = _target - _trailRB.transform.position;
           speed = _targetRB.velocity.magnitude;
           if(speedMultiplier < 1){
               speedMultiplier+= Time.deltaTime/accelerationTime;
               speedMultiplier = Mathf.Clamp(speedMultiplier,0,1);
           }
           if(RubberBanding()){
               aditionalMult = 1.5f;
           }
           else if(Vector3.Distance(_trailRB.transform.position,transform.position) < distance - .1f){
               aditionalMult = .2f;        
           }
           else{
               aditionalMult = 1;
           }
           inBetween.transform.position =  Vector3.Lerp(transform.position, _trailRB.transform.position,.5f);
       }
    }
    private void FixedUpdate() {
        if(!locked){
            _trailRB.velocity = direction.normalized * speed * speedMultiplier * aditionalMult;
        }
        else{
            _trailRB.velocity = Vector3.zero;
        }
    }
    public void Lock (){
        locked = true;   
        _trailRB.transform.position = transform.position;    
        _trailRB.isKinematic = true;
    }
    public void Unlock (){
        locked = false;      
        _trailRB.isKinematic = false;
        speedMultiplier =.1f;
    }

    bool RubberBanding(){
        float distanceFromTarget = Vector3.Distance(_trailRB.transform.position,_target);
        float distanceFromParent = Vector3.Distance(_trailRB.transform.position,transform.position);
        return distanceFromTarget > distance && distanceFromParent > distance + .1f;
    }

    public void ResetBetweenPosition(){
        locked = true;
        inBetween.transform.position = transform.position;
    }
}
