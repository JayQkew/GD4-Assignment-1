using UnityEngine;
using UnityEngine.InputSystem;

public class MoveCubeAgain: MonoBehaviour
{

    public float moveSpeedAgain = 5;
    private Vector2 cubeDirection;

    public void MoveTheCube(InputAction.CallbackContext ctx)
    {
        if (ctx.performed == true)
        {
            Vector2 playerInput = ctx.ReadValue<Vector2>();
            cubeDirection.x = playerInput.x;
            cubeDirection.y = playerInput.y;
        }
        else
        {
            cubeDirection = Vector2.zero;
        }
    }

    public void Update()
    {
        Vector2 movement = new Vector2(cubeDirection.x, 0) * moveSpeedAgain * Time.deltaTime;
        transform.Translate(movement);
    }
}


