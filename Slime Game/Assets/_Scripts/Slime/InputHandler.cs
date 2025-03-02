using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class InputHandler : MonoBehaviour
{
    public InputActionAsset inputAsset;
    public PlayerInput playerInput;
    public MovementType movementType;
    public InputMode inputMode;

    private InputActionMap movmentMap_1;
    private InputActionMap movmentMap_2;

    private InputAction moveAction;
    private InputAction stiffnessAction;
    private InputAction jumpAction;

    [Header("Movement 1")]
    public Vector2 moveInput;
    public int stiffnessInput;

    [Header("Movement 2")] 
    public float xAxisClamp;
    public Vector2 aimInput;
    public int inflateInput;
    public bool jumpInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        inputMode = GetInputMode();
        
        movmentMap_1 = inputAsset.FindActionMap("Movement_1");
        movmentMap_2 = inputAsset.FindActionMap("Movement_2");
        
        moveAction = movmentMap_1.FindAction("Move");
        stiffnessAction = movmentMap_1.FindAction("Stiffness");
        jumpAction = movmentMap_2.FindAction("Jump");

        // movmentMap_2 = inputAsset.FindActionMap("Movement_2");
    }

    private void OnEnable()
    {
        inputAsset.Enable();

        switch (movementType)
        {
            case MovementType.Movement1:
                movmentMap_1.Enable();
                // movmentMap_2.Disable();

                moveAction.Enable();
                stiffnessAction.Enable();
                break;
            case MovementType.Movement2:
                movmentMap_1.Disable();
                // movmentMap_2.Enable();

                moveAction.Disable();
                stiffnessAction.Disable();
                break;
        }
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
    #endregion
    private InputMode GetInputMode()
    {
        if (playerInput.currentControlScheme == "Gamepad") return InputMode.Gamepad;
        if (playerInput.currentControlScheme == "Keyboard&Mouse") return InputMode.KeyboardMouse;
        if (playerInput.currentControlScheme == "Joystick") return InputMode.Joystick;

        return InputMode.KeyboardMouse;
    }

    public enum MovementType
    {
        Movement1,
        Movement2
    }

    public enum InputMode
    {
        KeyboardMouse,
        Gamepad,
        Joystick
    }
}