using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private InputMap _myInputs;
    //--------------------------Jump
    public bool jumpPerformed;
    public bool jumpHold;
    //--------------------------
    private void Awake(){
        _myInputs = new InputMap();
        _myInputs.Gameplay.Jump.performed += ctx => JumpButtonUp();
        _myInputs.Gameplay.Jump.canceled += ctx => JumpButtonDown();
    }
    private void OnEnable(){
        _myInputs.Gameplay.Axis.Enable();
        _myInputs.Gameplay.Jump.Enable();
    }
    private void OnDisable(){
        _myInputs.Gameplay.Axis.Disable();
        _myInputs.Gameplay.Jump.Disable();
    }
    public Vector2 axis(){
        return _myInputs.Gameplay.Axis.ReadValue<Vector2>().normalized;
    }
    private void JumpButtonUp(){
        jumpPerformed = true;
        jumpHold = true;
    }
    private void JumpButtonDown()
    {
        jumpHold = false;
    }
}
