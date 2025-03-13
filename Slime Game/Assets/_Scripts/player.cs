using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerOneScript : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeedAgain = 5f;
    public float jumpForce = 7f;
    private Vector2 cubeDirection;
    private Rigidbody2D rb;
    [Header("Shooting Settings")]
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float projectileSpeed = 10f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Ensure cube has a Rigidbody component
    }
    public void MovePlayerOne(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Vector2 playerInput = ctx.ReadValue<Vector2>();
            cubeDirection.x = playerInput.x;
            cubeDirection.y = playerInput.y;
        }
        else if (ctx.canceled)
        {
            cubeDirection = Vector2.zero;
        }
    }
    public void JumpPlayerOne(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && Mathf.Abs(rb.linearVelocity.y) < 0.01f)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
    public void ShootPlayerOne(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            GameObject projectile = Instantiate(projectilePrefab,
            shootPoint.position, Quaternion.identity);
            Rigidbody2D projRb = projectile.GetComponent<Rigidbody2D>();
            projRb.linearVelocity = transform.forward * projectileSpeed;
            Destroy(projectile, 3f);
        }
    }
    private void Update()
    {
        Vector2 movement = new Vector2(cubeDirection.x, cubeDirection.y) *
        moveSpeedAgain * Time.deltaTime;
        transform.Translate(movement);
    }
}
