using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBrain : MonoBehaviour
{
    [SerializeField]
    private Transform[] _groundCheck;
    [SerializeField]
    private Transform[] _wallCheck;
    [SerializeField]
    private float _floorDetection;
    [SerializeField]
    private LayerMask _walkable;
    private InputManager _myInputs;
    private Rigidbody _rb;
    private Movement _myMovement;
    private CharacterVisuals _myVisuals;
    private bool facingRight;
    private bool wasFacingRight;

    private void Awake(){
        facingRight = (transform.position.x + transform.right.x) > transform.position.x;
        wasFacingRight = facingRight;
    }
    private void Start(){
        _myInputs = GetComponent<InputManager>();
        _rb = GetComponent<Rigidbody>();
        _myMovement = GetComponent<Movement>();
        _myVisuals = GetComponent<CharacterVisuals>();
    }
    private void Update(){
        if (_myInputs.jumpPerformed){
            if (Grounded()){
                _myMovement.Jump();
            }
            _myInputs.jumpPerformed = false;
        }
        if(_myInputs.axis().x!=0)
        {
            facingRight = _myInputs.axis().x > 0;
            if (wasFacingRight != facingRight)
            {
                _myVisuals.Flip();
                _myMovement.SetAcceleration(0);
                wasFacingRight = facingRight;
            }
        }
        if(OnWall() && _myInputs.axis().x != 0)
        {
            _myMovement.SetAcceleration(0);
        }
    }
    private void FixedUpdate(){
        _myMovement.Move(_myInputs.axis().x);
        if (!Grounded())
        {
            _myMovement.SetFallSpeed(_myInputs.jumpHold);
        }
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
    bool OnWall()
    {
        int counter = 0;
        for (int i = 0; i < _wallCheck.Length; i++)
        {
            if (Physics.Raycast(_wallCheck[i].position, transform.right, _floorDetection, _walkable))
            {
                counter++;
            }
        }
        return counter != 0;
    }
    private void OnDrawGizmos(){
        for (int i = 0; i < _groundCheck.Length; i++){
            Gizmos.DrawLine(_groundCheck[i].position, _groundCheck[i].position + -Vector3.up * _floorDetection);
        }
        for (int i = 0; i < _groundCheck.Length; i++)
        {
            Gizmos.DrawLine(_wallCheck[i].position, _wallCheck[i].position + transform.right * _floorDetection);
        }
    }
}
