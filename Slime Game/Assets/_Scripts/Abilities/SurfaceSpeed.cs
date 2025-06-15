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
