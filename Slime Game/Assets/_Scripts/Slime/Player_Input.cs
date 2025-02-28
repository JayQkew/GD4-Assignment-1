using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Input : MonoBehaviour
{
    private Movement _movement;
    
    private InputAction moveAction;
    public Vector2 moveDirection;
    public InputAction sprintAction;

    private void Awake()
    {
        _movement = GetComponent<Movement>();
        
        moveAction = InputSystem.actions.FindAction("Move");
        sprintAction = InputSystem.actions.FindAction("Jump");
    }

    private void Update()
    {
        moveDirection = moveAction.ReadValue<Vector2>();
        _movement.AdjustGravity(sprintAction.IsPressed());
    }

    private void FixedUpdate()
    {
        _movement.Move(moveDirection);
    }
}
