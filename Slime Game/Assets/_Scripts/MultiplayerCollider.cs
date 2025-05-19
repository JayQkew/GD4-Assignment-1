using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiplayerCollider : MonoBehaviour
{
    private PlayerInputManager playerInputManager;
    private Movement[] playerMovements;
    private PolygonCollider2D[] playerColliders;
    private SoftBody[] playerSoftBodies;

    private void Awake() => playerInputManager = GetComponent<PlayerInputManager>();

    private void Update() {
        if (playerInputManager.playerCount == 2) PreventOverlap();
    }

    public void OnPlayerJoined(PlayerInput playerInput) {
        playerMovements[playerInputManager.playerCount] = playerInput.GetComponentInChildren<Movement>();
        playerColliders[playerInputManager.playerCount] = playerInput.GetComponentInChildren<PolygonCollider2D>();
        playerSoftBodies[playerInputManager.playerCount] = playerInput.GetComponentInChildren<SoftBody>();
    }

    private void PreventOverlap() {
        ColliderDistance2D distance = Physics2D.Distance(playerColliders[0], playerColliders[1]);
        if (distance.isOverlapped) {
            playerSoftBodies[0].AddForce(-distance.normal * Mathf.Abs(distance.distance));
            playerSoftBodies[1].AddForce(distance.normal * Mathf.Abs(distance.distance));
        }
    }
}
