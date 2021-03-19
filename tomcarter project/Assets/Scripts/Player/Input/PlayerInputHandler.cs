using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{

    public Vector2 rawAxis { get; private set; }
    public Vector2Int  FixedAxis { get; private set; }
    public int normX { get; private set; }
    public int normY { get; private set; }
    public bool jumpInput { get; private set; }
    public bool jumpStop { get; private set; }
    public bool dashInput { get; private set; }
    public bool dashStop { get; private set; }
    public bool hookInput { get; private set; }


    [SerializeField]
    private float inputHoldTime;  
    private float jumpInputStartTime;
    private float dashInputStartTime;
    private float hookInputStartTime;

    private void Update()
    {
        JumpHoldTime();
        DashHoldTime();
        HookHoldTime();
    }

    public void OnMoveInput(InputAction.CallbackContext ctx)
    {
        rawAxis = ctx.ReadValue<Vector2>();
        FixedAxis = Vector2Int.RoundToInt(rawAxis.normalized);
    }

    public void OnJumpInput(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            jumpInput = true;
            jumpStop = false;
            jumpInputStartTime = Time.time;
        }
        else if (ctx.canceled)
        {
            jumpStop = true;
        }
    }

    public void OnDashInput(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            dashInput = true;
            dashStop = false;
            dashInputStartTime = Time.time;
        }
        else if (ctx.canceled)
        {
            dashStop = true;
        }
    }

    public void OnHookInput(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            hookInput = true;
            hookInputStartTime = Time.time;
        }
    }

    private void JumpHoldTime()
    {
        if (Time.time >= jumpInputStartTime + inputHoldTime)
        {
            Jumped();
        }
    }
    private void DashHoldTime()
    {
        if (Time.time >= dashInputStartTime + inputHoldTime)
        {
            Dashed();
        }
    }
    private void HookHoldTime()
    {
        if (Time.time >= hookInputStartTime + inputHoldTime)
        {
            Hooked();
        }
    }

    public void Jumped() => jumpInput = false;
    public void Dashed() => dashInput = false;
    public void Hooked() => hookInput = false;


}
