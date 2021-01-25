using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBrain : MonoBehaviour
{
    public Transform [] myGroundCheck;
    public float groundCheckDistance;
    public LayerMask walkable;
    private CharacterInput _myInputs;
    private PlayerMovement _myMovement;
    private PlayerJump _myJump;
    private Rigidbody _myRigidbody;
    private bool _wasGrounded;
    private TrailRenderer _jumpTrail;


    private void Start(){
        _myInputs = GetComponent<CharacterInput>();
        _myMovement = GetComponent<PlayerMovement>();
        _myJump = GetComponent<PlayerJump>();
        _myRigidbody = GetComponent<Rigidbody>();
        _wasGrounded = Grounded();
        _jumpTrail = GetComponent<TrailRenderer>();
    }
    private void Update(){
        if (_myInputs.jumpPerformed){
            if (Grounded())
            {
                _myJump.Jump();
            }         
            _myInputs.jumpPerformed = false;
        }
        _myJump.jumpData(_myInputs.holdJump, Grounded());
        if (_wasGrounded != Grounded()){
            LandEvent();
        }
        _jumpTrail.enabled = !Grounded();
    }
    private void FixedUpdate(){
        _myMovement.Movement(_myInputs.axis.x, Grounded());
    }
    private bool Grounded(){
        int rays = 0;
        foreach (var check in myGroundCheck){
            if (Physics.Raycast(check.transform.position, -Vector3.up, groundCheckDistance, walkable)){
                rays++;
            }
        }      
        return rays != 0;      
    }
    void LandEvent(){
        if (Grounded()){
            _myMovement.ForceStop();
            _myJump.OnLanding();
            _jumpTrail.Clear();
        }
        else{
        }
        _wasGrounded = Grounded();
    }

    private void OnDrawGizmos()
    {
        foreach (var check in myGroundCheck)
        {
            Gizmos.DrawLine(check.transform.position, check.transform.position - Vector3.up * groundCheckDistance);
        }
    }
}
