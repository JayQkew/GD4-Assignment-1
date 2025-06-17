using System;
using UnityEngine;

public class VacuumChamber : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private float radius;
    [SerializeField] private float pullForce;
    
    private Movement movement;
    private PlayerStats playerStats;
    private InputHandler inputHandler;
    private SoftBody softBody;

    private void Awake() {
        movement = transform.parent.parent.GetComponentInChildren<Movement>();
        playerStats = transform.parent.parent.GetComponentInChildren<PlayerStats>();
        inputHandler = transform.parent.parent.GetComponent<InputHandler>();
        softBody = playerStats.GetComponentInChildren<SoftBody>();
        
        // inputHandler.onDash.RemoveListener(movement.Dash);
        inputHandler.onDash.AddListener(SpawnVacuum);
    }

    private void SpawnVacuum() {
        if (movement.currFuel >= playerStats.GetStatValue(StatName.DashCost)) {
            movement.currFuel -= playerStats.GetStatValue(StatName.DashCost);
            
            Vector2 pos = softBody.transform.position;
            
            Collider2D[] hits = Physics2D.OverlapCircleAll(pos, radius, targetLayers);
            foreach (Collider2D h in hits) {
                Rigidbody2D rb = h.transform.GetComponent<Rigidbody2D>();
                Debug.Log(h.transform.name);
                if (rb != null) {
                    Vector2 direction = (pos - rb.position).normalized;
                    rb.AddForce(direction * pullForce, ForceMode2D.Impulse);
                }
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(softBody.transform.position, radius);
    }
}
