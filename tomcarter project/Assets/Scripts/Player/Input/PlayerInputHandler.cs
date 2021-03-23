using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
      //-------------------------------------------------------------Variables
    #region Movement
        public Vector2 rawAxis { get; private set; }
        public Vector2Int  FixedAxis { get; private set; }
    #endregion
    #region Jump
        public bool JumpInput {get ; private set;}
        public bool JumpCancel {get ; private set;}
        private float _jumpStartTime;
    #endregion
    #region Roots
        public bool RootsInput {get ; private set;}
        public bool RootsCancel {get ; private set;}
        private float _rootsStartTime;
    #endregion
    #region Dash
        public bool DashInput {get ; private set;}
        public bool DashCancel {get ; private set;}
        private float _dashStartTime;
    #endregion
    #region Melee
        public bool MeleeInput {get ; private set;}
        public bool MeleeCancel {get ; private set;}
        private float _meleeStartTime;
    #endregion
    #region Range
        public bool RangeInput {get ; private set;}
        public bool RangeCancel {get ; private set;}
        private float _rangeStartTime;
    #endregion
    #region Hook
        public bool HookInput {get ; private set;}
        public bool HookCancel {get ; private set;}
        private float _hookStartTime;
    #endregion
    #region Guard
        public bool GuardInput {get ; private set;}
        public bool GuardCancel {get ; private set;}
        private float _guardStartTime;
    #endregion
    #region Interaction
        public bool InteractionInput {get ; private set;}
        public bool InteractionCancel {get ; private set;}
        private float _interactionStartTime;
    #endregion
    [SerializeField]
    private float _inputHoldTime;
    //-------------------------------------------------------------Functions
    private void Update() => holdTime();
    public void OnMovementInput(InputAction.CallbackContext ctx)
    {
        rawAxis = ctx.ReadValue<Vector2>();
        FixedAxis = Vector2Int.RoundToInt(rawAxis.normalized);
        Debug.Log("movement");
    }
    public void OnJumpInput(InputAction.CallbackContext ctx)
    {
        if(ctx.started)
        {
            JumpInput = true;
            JumpCancel = false;
            _jumpStartTime = Time.time;
            Debug.Log("jump");
        } 
        if(ctx.canceled)
        {
            JumpCancel = true;
        }      
    }
    public void OnRootsInput(InputAction.CallbackContext ctx)
    {
        if(ctx.started)
        {
            RootsInput = true;
            RootsCancel = false;
            _rootsStartTime = Time.time;
            Debug.Log("roots");
        } 
        if(ctx.canceled)
        {
            RootsCancel = true;
        }      
    }
    public void OnDashInput(InputAction.CallbackContext ctx)
    {
        if(ctx.started)
        {
            DashInput = true;
            DashCancel = false;
            _dashStartTime = Time.time;
            Debug.Log("dash");
        } 
        if(ctx.canceled)
        {
            DashCancel = true;
        }    
    }
    public void OnMeleeInput(InputAction.CallbackContext ctx)
    {
        if(ctx.started)
        {
            MeleeInput = true;
            MeleeCancel = false;
            _meleeStartTime = Time.time;
            Debug.Log("melee");
        } 
        if(ctx.canceled)
        {
            MeleeCancel = true;
        }      
    }
    public void OnRangeInput(InputAction.CallbackContext ctx)
    {
        if(ctx.started)
        {
            RangeInput = true;
            RangeCancel = false;
            _rangeStartTime = Time.time;
            Debug.Log("range");
        } 
        if(ctx.canceled)
        {
            RangeCancel = true;
        }        
    }
    public void OnHookInput(InputAction.CallbackContext ctx)
    {
        if(ctx.started)
        {
            HookInput = true;
            HookCancel = false;
            _hookStartTime = Time.time;
            Debug.Log("hook");
        } 
        if(ctx.canceled)
        {
            HookCancel = true;
        }        
    }
    public void OnGuardInput(InputAction.CallbackContext ctx)
    {
        if(ctx.started)
        {
            GuardInput = true;
            GuardCancel = false;
            _guardStartTime = Time.time;
            Debug.Log("guard");
        } 
        if(ctx.canceled)
        {
            GuardCancel = true;
        }  
    }
    public void OnInteractionInput(InputAction.CallbackContext ctx)
    {
        if(ctx.started)
        {
            InteractionInput = true;
            InteractionCancel = false;
            _interactionStartTime = Time.time;
            Debug.Log("interaction");
        } 
        if(ctx.canceled)
        {
            InteractionCancel = true;
        }     
    }
    private void holdTime()
    {
        if(Time.time > _jumpStartTime + _inputHoldTime)
        {
            JumpInput = false;
        }
        if(Time.time > _rootsStartTime + _inputHoldTime)
        {
            RootsInput = false;
        }
        if(Time.time > _dashStartTime + _inputHoldTime)
        {
            DashInput = false;
        }
        if(Time.time > _meleeStartTime + _inputHoldTime)
        {
            MeleeInput = false;
        }
        if(Time.time > _rangeStartTime + _inputHoldTime)
        {
            RangeInput = false;
        }
        if(Time.time > _hookStartTime + _inputHoldTime)
        {
            HookInput = false;
        }
        if(Time.time > _guardStartTime + _inputHoldTime)
        {
            GuardInput = false;
        }
        if(Time.time > _interactionStartTime + _inputHoldTime)
        {
            InteractionInput = false;
        }        
    }
    public void UsedJump() => JumpInput = false;
    public void UsedRoots() => RootsInput = false;
    public void UsedDash() => DashInput = false;
    public void UsedMelee() => MeleeInput = false;
    public void UsedRange() => RangeInput = false;
    public void UsedHook() => HookInput = false;
    public void UsedGuard() => GuardInput = false;
    public void UsedInteraction() => InteractionInput = false;

}
