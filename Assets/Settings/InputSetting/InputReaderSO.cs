using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "SO/InputReader")]
public class InputReaderSO : ScriptableObject, Controls.IPlayerActions
{
    private Controls _controls;

    #region Actions

    public event Action OnMeleeAttackEvent;
    public event Action OnInteractEvent;
    public event Action OnJumpEvent;
    public event Action OnDashEvent;
    public event Action OnWeaponSawpEvent;
    public event Action<bool> OnCrouchEvent;
    public event Action<bool> OnAttackEvent;

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
            _controls.Player.SetCallbacks(this);
        }

        _controls.Player.Enable();
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
            OnWeaponSawpEvent?.Invoke();
    }
}
