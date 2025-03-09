using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class InputHandler : MonoBehaviour
{
    public PlayerInput playerInput;
    public InputMode inputMode;
    private GameObject softBody;
    
    [Header("Small Slime Movement")]
    public float xAxisClamp;
    public Vector2 aimInput;
    public bool jumpInput;
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