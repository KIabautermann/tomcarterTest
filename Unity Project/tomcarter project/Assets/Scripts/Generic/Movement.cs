using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //--------------------------------Basic Movement
    [SerializeField]
    private float _movementSpeed;
    [SerializeField]
    private float _accelerationTime;
    private float _accelerationIndex;
    private Rigidbody _rb;
    private float direction;
    //--------------------------------Jump
    [SerializeField]
    private float _jumpForce;
    [SerializeField]
    private float _fallMultiplier;
    [SerializeField]
    private float _shortHopForce;
    [SerializeField]
    private float _minJumpSpeed;



    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Move(float axisX)
    {
        if (axisX != 0){
            direction = axisX;
        }
        if (_accelerationTime != 0){
            _accelerationIndex += (axisX != 0 ? 1 / _accelerationTime : -1 / _accelerationTime) * Time.deltaTime;
        }
        else{
            _accelerationIndex = 1;
        }       
        _accelerationIndex = Mathf.Clamp(_accelerationIndex, 0, 1);
        float xVel = direction * _movementSpeed * _accelerationIndex;
        _rb.velocity = new Vector3(xVel, _rb.velocity.y, 0);
    }
    public void Jump(){
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }
    public void SetFallSpeed(bool holding)
    {
        if (_rb.velocity.y>_minJumpSpeed && holding){
            return;
        }
        else{
            if(_rb.velocity.y <= _minJumpSpeed){
                _rb.AddForce(Physics.gravity * _fallMultiplier);
            }
            else if(_rb.velocity.y > _minJumpSpeed && !holding){
                _rb.AddForce(Physics.gravity * _shortHopForce);
            }
        }
    }

    public void SetAcceleration(float index)
    {
        index = Mathf.Clamp(index, 0, 1);
        _accelerationIndex = index;
    }
}
