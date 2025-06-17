using System;
using UnityEngine;

public class SurfaceSpeed : MonoBehaviour
{
    private Movement playerMovement;
    private PlayerStats playerStats;
    public float currValue;
    public float multValue;

    private void Awake() {
        playerMovement = transform.parent.parent.GetComponentInChildren<Movement>();
        playerStats = playerMovement.GetComponent<PlayerStats>();
        
        SurfaceSpeed[] thisAbility = transform.parent.GetComponentsInChildren<SurfaceSpeed>();
        if (thisAbility.Length >= 2) {
            foreach (SurfaceSpeed surfaceSpeed in thisAbility) {
                if (surfaceSpeed != this) {
                    surfaceSpeed.multValue *= multValue;
                    Destroy(gameObject);
                }
            }
        }
        if (playerMovement.surfaceSpeed == null) {
            playerMovement.surfaceSpeed = this;
        }
    }

    private void Update() {
        currValue = playerMovement.TouchingSurface() ? 
            playerStats.GetStatValue(StatName.MoveSpeed) * multValue :
            playerStats.GetStatValue(StatName.MoveSpeed);
    }
}
