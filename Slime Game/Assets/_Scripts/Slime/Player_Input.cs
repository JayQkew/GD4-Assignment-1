using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Input : MonoBehaviour
{
    private SoftBody_MovementInterface _softBodyMovementInterface;
    
    private InputAction moveAction;
    public Vector2 moveDirection;
    public InputAction sprintAction;

    private void Awake()
    {
        _softBodyMovementInterface = GetComponent<SoftBody_MovementInterface>();
        
        moveAction = InputSystem.actions.FindAction("Move");
        sprintAction = InputSystem.actions.FindAction("Sprint");
    }

    private void Update()
    {
        moveDirection = moveAction.ReadValue<Vector2>();
        _softBodyMovementInterface.AdjustGravity(sprintAction.IsPressed());
    }

    private void FixedUpdate()
    {
        _softBodyMovementInterface.Move(moveDirection);
    }
}
