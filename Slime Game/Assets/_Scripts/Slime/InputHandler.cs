using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class InputHandler : MonoBehaviour
{
    public InputActionAsset inputAsset;
    [SerializeField] private MovementType movementType;

    private InputActionMap movmentMap_1;
    private InputActionMap movmentMap_2;
    
    private InputAction moveAction;
    private InputAction stiffnessAction;

    public Vector2 moveInput;
    public double stiffnessInput;
    private void Awake()
    {
        movmentMap_1 = inputAsset.FindActionMap("Movement_1");
        
        moveAction = movmentMap_1.FindAction("Move");
        moveAction.performed += Move;
        moveAction.canceled += Move;
        
        stiffnessAction = movmentMap_1.FindAction("Stiffness");
        stiffnessAction.performed += Stiffness;
        stiffnessAction.canceled += Stiffness;
        
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
    
    private void Stiffness(InputAction.CallbackContext ctx)
    {
        stiffnessInput = ctx.ReadValue<float>();
    }

    private void Move(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    private enum MovementType
    {
        Movement1,
        Movement2
    }
}


