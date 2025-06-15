using System;
using UnityEngine;

public class SpitBall : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float projectileForce;
    
    private InputHandler inputHandler;
    private PlayerStats playerStats;
    private Movement movement;
    private SoftBody softBody;

    private void Awake() {
        inputHandler = transform.parent.parent.GetComponent<InputHandler>();
        playerStats = transform.parent.parent.GetComponentInChildren<PlayerStats>();
        movement = playerStats.GetComponent<Movement>();
        softBody = playerStats.GetComponentInChildren<SoftBody>();
        
        inputHandler.onDash.AddListener(ShootProjectile);
        inputHandler.onDash.RemoveListener(movement.Dash);
    }
    
    private void ShootProjectile() {
        if (movement.currFuel >= playerStats.GetStatValue(StatName.DashCost)) {
            movement.currFuel -= playerStats.GetStatValue(StatName.DashCost);
            Vector2 dir = inputHandler.aimInput;
            float totalForce = playerStats.GetStatValue(StatName.DashForce) + projectileForce;
            
            Vector2 spawnPos = softBody.transform.position;
            
            GameObject projectileInstance = Instantiate(projectile, spawnPos, Quaternion.identity);
            projectileInstance.GetComponent<Rigidbody2D>().linearVelocity =
                softBody.GetComponent<Rigidbody2D>().linearVelocity;
            projectileInstance.GetComponent<Rigidbody2D>().AddForce(dir * totalForce, ForceMode2D.Impulse);
        }
    }
}
