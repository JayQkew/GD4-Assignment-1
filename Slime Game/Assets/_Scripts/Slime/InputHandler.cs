using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class InputHandler : MonoBehaviour
{
    public PlayerInput playerInput;
    public InputMode inputMode;

    [HideInInspector] public UnityEvent OnJumpPressed;
    [HideInInspector] public UnityEvent OnGrab;
    [HideInInspector] public UnityEvent OnRelease;
    
    public Vector2 moveInput;
    public Vector2 aimInput;

    private GameObject _softBody;
    

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        inputMode = GetInputMode();
        _softBody = transform.GetChild(0).gameObject;
    }
    
    public void Move(InputAction.CallbackContext ctx)
    {
        aimInput = ctx.ReadValue<Vector2>();
    }

    public void Aim(InputAction.CallbackContext ctx)
    {
        if (inputMode == InputMode.KeyboardMouse)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(ctx.ReadValue<Vector2>());
            Vector2 dir = mousePos - (Vector2)_softBody.transform.position;
            aimInput = Vector2.ClampMagnitude(dir, 1);
        }
        else
        {
            aimInput = ctx.ReadValue<Vector2>();
        }
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            OnJumpPressed?.Invoke();
        }
    }

    public void Grab(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            OnGrab?.Invoke();
        }
        else if (ctx.canceled)
        {
            OnRelease?.Invoke();
        }
    }
    
    private InputMode GetInputMode()
    {
        if (playerInput.currentControlScheme == "Gamepad") return InputMode.Gamepad;
        if (playerInput.currentControlScheme == "Keyboard&Mouse") return InputMode.KeyboardMouse;
        if (playerInput.currentControlScheme == "Joystick") return InputMode.Joystick;

        return InputMode.KeyboardMouse;
    }

    public enum InputMode
    {
        KeyboardMouse,
        Gamepad,
        Joystick
    }
}