using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "SO/InputReader")]
public class InputReaderSO : ScriptableObject, Controls.IPlayerActions, Controls.IUIActions
{
    private Controls _controls;
    public Controls.PlayerActions PlayerActions { get; private set; }
    public Controls.UIActions UIActions { get; private set; }
    
    #region Actions

    public event Action OnMeleeAttackEvent;
    public event Action OnInteractEvent;
    public event Action OnJumpEvent;
    public event Action OnDashEvent;
    public event Action OnWeaponSwapEvent;
    public event Action OnReloadEvent;
    public event Action<bool> OnCrouchEvent;
    public event Action<bool> OnAttackEvent;
    public event Action<Vector2> OnNavigateEvent;
    public event Action OnStatusWindowEnableEvent;
    public event Action<float> OnStatusTapMoveToSideEvent;
    
    #endregion

    #region Values

    public Vector2 MousePosition { get; private set; }
    public float XMovement { get; private set; }

    #endregion

    private void OnEnable()
    {
        if (_controls == null)
        {
            _controls = new Controls();
            PlayerActions = _controls.Player;
            UIActions = _controls.UI;
            
            PlayerActions.SetCallbacks(this);
            UIActions.SetCallbacks(this);
        }

        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnAttackEvent?.Invoke(true);
        else if (context.canceled)
            OnAttackEvent?.Invoke(false);
    }

    public void OnMeleeAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnMeleeAttackEvent?.Invoke();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnInteractEvent?.Invoke();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnJumpEvent?.Invoke();
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnCrouchEvent?.Invoke(true);
        else if (context.canceled)
            OnCrouchEvent?.Invoke(false);
    }

    public void OnMousePosition(InputAction.CallbackContext context)
    {
        MousePosition = context.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        XMovement = context.ReadValue<float>();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnDashEvent?.Invoke();
    }

    public void OnWeaponSwap(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnWeaponSwapEvent?.Invoke();
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnReloadEvent?.Invoke();
    }
    
    #region UI
    public void OnStatusWindowEnable(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnStatusWindowEnableEvent?.Invoke();
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
        if(context.performed)
            OnNavigateEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
    }

    public void OnClick(InputAction.CallbackContext context)
    {
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
    }

    public void OnMiddleClick(InputAction.CallbackContext context)
    {
    }

    public void OnScrollWheel(InputAction.CallbackContext context)
    {
    }

    public void OnTrackedDevicePosition(InputAction.CallbackContext context)
    {
    }

    public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
    {
    }

    public void OnStatusTapMoveToSide(InputAction.CallbackContext context)
    {
        if (context.performed)
           OnStatusTapMoveToSideEvent?.Invoke(context.ReadValue<float>()); 
    }

    #endregion
}
