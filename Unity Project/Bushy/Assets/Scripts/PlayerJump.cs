using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class PlayerJump : MonoBehaviour
{
    private Rigidbody _myRigidbody;
    public float jumpForce; 
    public float minJumpSpeed;
    public float fallMultiplier;
    public float shortHopMultiplier;
    private float currentMultiplier;
    private bool _holdingJump;
    private bool _fastFall;
    private float _initialPos;
    private bool _measure;
    private bool _grounded;

    private void Start(){
        _myRigidbody = GetComponent<Rigidbody>();
    }
    public void Jump(){

        _initialPos = transform.position.y;
        _myRigidbody.velocity = Vector3.zero;
        _myRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        _measure = true;
    }
    public void jumpData(bool holding, bool grounded){
        _holdingJump = holding;
        _grounded = grounded;
    }
    private void Update(){
        if (_myRigidbody.velocity.y <= 0 && _measure && transform.position.y != _initialPos){
            _measure = false;
        }
        if (_fastFall && _grounded){
            _fastFall = false;
        }  
        else{
            if (!_grounded)
            {
                if (_myRigidbody.velocity.y < minJumpSpeed){
                    currentMultiplier = fallMultiplier;
                    _fastFall = true;
                }
                else if (_myRigidbody.velocity.y > 0 && !_holdingJump){
                    currentMultiplier = shortHopMultiplier;
                    _fastFall = true;
                }
            }          
        }       
    }
    private void FixedUpdate(){
        if(_fastFall){
            _myRigidbody.AddForce(Physics.gravity * currentMultiplier, ForceMode.Acceleration);
        }
    }
    public void OnLanding(){
        _fastFall = false;
    }
}
