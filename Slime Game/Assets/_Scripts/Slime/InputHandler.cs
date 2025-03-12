using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class InputHandler : MonoBehaviour
{
    public PlayerInput playerInput;

    public UnityEvent OnJumpPressed;
    public UnityEvent OnGrab;
    public UnityEvent OnRelease;
    
    [Header("Small Slime Movement")]
    public Vector2 aimInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }
    
    public void Move(InputAction.CallbackContext ctx)
    {
        aimInput = ctx.ReadValue<Vector2>();
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

    public enum InputMode
    {
        KeyboardMouse,
        Gamepad,
        Joystick
    }
}