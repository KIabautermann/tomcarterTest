using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _myRigidbody;
    public float movementSpeed;
    public float groundedAccelerationTime;
    public float aerialAccelerationTime;
    private float _myDirection;
    private bool _facingRight;
    private bool _wasFacingRight;
    private float _accelerationIndex;

    private void Start(){
        _myRigidbody = GetComponent<Rigidbody>();
    }
    public void Movement(float direction, bool grounded){
        if(direction != 0){
            _myDirection = direction;
        }
        _accelerationIndex += (1 / (grounded ? groundedAccelerationTime : aerialAccelerationTime)) * Time.deltaTime * (direction != 0 ? 1 : -1);
        _accelerationIndex = Mathf.Clamp(_accelerationIndex, 0, 1);
        _facingRight = _myDirection < 0;       
        if(_wasFacingRight!= _facingRight && direction != 0){
            transform.Rotate(0, 180, 0);
            _accelerationIndex = 0;
            _wasFacingRight = _facingRight;
        }
        _myRigidbody.MovePosition(transform.position + Vector3.right * _myDirection * _accelerationIndex * movementSpeed * Time.fixedDeltaTime);
    }

    public void ForceStop(){
        _myRigidbody.velocity = new Vector3(0, _myRigidbody.velocity.y, 0);
    }


}
