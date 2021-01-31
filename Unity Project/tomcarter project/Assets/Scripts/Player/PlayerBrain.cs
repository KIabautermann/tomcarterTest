using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBrain : MonoBehaviour
{
    [SerializeField]
    private Transform[] _groundCheck;
    [SerializeField]
    private float _floorDetection;
    [SerializeField]
    private LayerMask _walkable;
    private InputManager _myInputs;
    private Rigidbody _rb;
    private Movement _myMovement;

    private void Start(){
        _myInputs = GetComponent<InputManager>();
        _rb = GetComponent<Rigidbody>();
        _myMovement = GetComponent<Movement>();
    }

    private void Update(){
        if (_myInputs.jumpPerformed){
            if (Grounded()){
                _myMovement.Jump();
            }
            _myInputs.jumpPerformed = false;
        }
    }
    private void FixedUpdate(){
        _myMovement.Move(_myInputs.axis().x);
    }

    private bool Grounded(){
        int counter = 0;
        for (int i = 0; i < _groundCheck.Length; i++){
            if(Physics.Raycast(_groundCheck[i].position, -Vector3.up, _floorDetection, _walkable)){
                counter++;
            }
        }
        return counter != 0;
    }

    private void OnDrawGizmos(){
        for (int i = 0; i < _groundCheck.Length; i++){
            Gizmos.DrawLine(_groundCheck[i].position, _groundCheck[i].position + -Vector3.up * _floorDetection);
        }
    }
}
