using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class InputHandler : MonoBehaviour
{
    public PlayerInput playerInput;
    public InputMode inputMode;
    private GameObject softBody;

    public UnityEvent OnJumpPressed;
    public UnityEvent OnGrab;
    public UnityEvent OnRelease;
    
    [Header("Small Slime Movement")]
    public Vector2 aimInput;
    public bool grabInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        inputMode = GetInputMode();
        softBody = transform.GetChild(0).gameObject;
    }
    
    public void Aim(InputAction.CallbackContext ctx)
    {
        if (inputMode == InputMode.KeyboardMouse)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(ctx.ReadValue<Vector2>());
            Vector2 dir = mousePos - (Vector2)softBody.transform.position;
            aimInput = new Vector2(dir.x, dir.y);
        }
        else
        {
            Vector2 dir = ctx.ReadValue<Vector2>();
            aimInput = new Vector2(dir.x, dir.y);
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