using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "SO/InputReader")]
public class InputReaderSO : ScriptableObject, Controls.IPlayerActions
{
    private Controls _controls;

    #region Actions

    public event Action OnAttackEvent;
    public event Action OnInteractEvent;
    public event Action OnJumpEvent;
    public event Action<bool> OnCrouchEvent;
    public event Action<bool> OnSprintEvent;

    #endregion

    #region Values

    public Vector2 MousePosition { get; private set; }
    public float XMovement { get; private set; }
    public bool IsSprint { get; private set; }

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
        _controls.Player.Disable();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnAttackEvent?.Invoke();
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

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsSprint = true;
            OnSprintEvent?.Invoke(true);
        }
        else if (context.canceled)
        {
            IsSprint = false;
            OnSprintEvent?.Invoke(false);
        }
    }

    public void OnMousePosition(InputAction.CallbackContext context)
    {
        MousePosition = context.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        XMovement = context.ReadValue<float>();
    }
}
