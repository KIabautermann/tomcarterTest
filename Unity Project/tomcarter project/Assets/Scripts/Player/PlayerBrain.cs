using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBrain : MonoBehaviour
{
    private InputManager _myInputs;
    private Rigidbody _rb;
    private PlayerMovement _myMovement;

    private void Start(){
        _myInputs = GetComponent<InputManager>();
        _rb = GetComponent<Rigidbody>();
        _myMovement = GetComponent<PlayerMovement>();
    }

    private void Update(){
        if (_myInputs.jumpPerformed){
            _myMovement.Jump();
            _myInputs.jumpPerformed = false;
        }
        if(_myInputs.axis().x != 0)
        {
            _myMovement.Movement(_myInputs.axis().x);
        }
    }
}
