using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class InputHandler : MonoBehaviour
{
    public InputActionAsset inputAsset;
    public PlayerInput playerInput;
    public InputMode inputMode;
    
    [Header("Movement 1")]
    public Vector2 moveInput;
    public int stiffnessInput;
    
    [Header("Small Slime Movement")]
    public float xAxisClamp;
    public Vector2 aimInput;
    public bool jumpInput;
    public bool grabInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        inputMode = GetInputMode();
    }
    
    #region Movement1
    public void Stiffness(InputAction.CallbackContext ctx)
    {
        stiffnessInput = (int)ctx.ReadValue<float>();
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }
    #endregion

    #region Movement2
    public void Aim(InputAction.CallbackContext ctx)
    {
        if (inputMode == InputMode.KeyboardMouse)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(ctx.ReadValue<Vector2>());
            Vector2 dir = mousePos - (Vector2)transform.position;
            Vector2 clamped = Vector2.ClampMagnitude(dir, 1);
            aimInput = new Vector2(clamped.x * xAxisClamp, clamped.y);
        }
        else
        {
            Vector2 clamped = Vector2.ClampMagnitude(ctx.ReadValue<Vector2>(), 1);
            aimInput = new Vector2(clamped.x * xAxisClamp, clamped.y);
        }
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        jumpInput = ctx.performed;
    }

    public void Grab(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) grabInput = true;
        else if (ctx.canceled) grabInput = false;
    }
    #endregion
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