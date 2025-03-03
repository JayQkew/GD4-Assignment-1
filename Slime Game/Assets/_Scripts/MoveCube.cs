using UnityEngine;
using UnityEngine.InputSystem;

public class MoveCube : MonoBehaviour
{
   

    public InputActionAsset inputActions;
    public float moveSpeed = 5f;

    private InputAction moveAction;
    private Vector2 moveInput;

    private void OnEnable()
    {
        // Find the action map and move action
        var actionMap = inputActions.FindActionMap("PlayerOne");
        moveAction = actionMap.FindAction("Movement");

        // Enable input actions
        actionMap.Enable();
        moveAction.Enable();

        // Subscribe to input performed/canceled events
        moveAction.performed += OnMove;
        moveAction.canceled += OnMoveCancelled;
    }

    private void OnDisable()
    {
        // Disable input actions and unsubscribe events
        moveAction.performed -= OnMove; // This listens for when the player presses a movement key like a or d
        moveAction.canceled -= OnMoveCancelled; // This listens for when the player releases the key
        moveAction.Disable(); // It turns off the input action -  Unity will stop listening for that specific input. Improves performance by stopping unnecessary input checks.
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        // Read movement input from the player when performed ^
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCancelled(InputAction.CallbackContext context)
    {
        // Reset movement when input is cancelled ^
        moveInput = Vector2.zero;
    }

    private void Update()
    {
        // Apply movement based on input
        Vector2 movement = new Vector2(moveInput.x, 0) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);
    }
}

