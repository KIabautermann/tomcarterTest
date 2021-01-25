using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterInput : MonoBehaviour
{
    private InputMap _inputMap;
    public Vector2 axis;
    public bool jumpPerformed;
    public bool holdJump;

    private void Awake(){
        _inputMap = new InputMap();
    }
    private void OnEnable() {
        _inputMap.Player.Axis.Enable();
        _inputMap.Player.Axis.performed += OnAxisMovement;
        _inputMap.Player.Jump.Enable();
        _inputMap.Player.Jump.performed += OnJumpPress;
        _inputMap.Player.Jump.canceled += OnJumpCancel;
    }
    private void OnAxisMovement(InputAction.CallbackContext ctx){
        axis = ctx.ReadValue<Vector2>();
    }

    private void OnJumpPress(InputAction.CallbackContext ctx){
        jumpPerformed = true;
        holdJump = true;
    }
    private void OnJumpCancel(InputAction.CallbackContext ctx){
        holdJump = false;
    }
    private void OnDisable(){
        _inputMap.Player.Axis.Disable();
        _inputMap.Player.Jump.Disable();
    }
}
