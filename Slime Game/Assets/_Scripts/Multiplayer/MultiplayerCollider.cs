using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiplayerCollider : MonoBehaviour
{
    private PlayerInputManager playerInputManager;
    [SerializeField] private Movement[] playerMovements = new Movement[2];
    public PolygonCollider2D[] playerColliders = new PolygonCollider2D[2];
    [SerializeField] private SoftBody[] playerSoftBodies = new SoftBody[2];

    private void Awake() => playerInputManager = GetComponent<PlayerInputManager>();

    private void Update() {
        if (playerColliders[0] && playerColliders[1]) PreventOverlap();
    }

    public void OnPlayerJoined(PlayerInput playerInput) {
        playerMovements[playerInputManager.playerCount - 1] = playerInput.GetComponentInChildren<Movement>();
        playerSoftBodies[playerInputManager.playerCount - 1] = playerInput.GetComponentInChildren<SoftBody>();
        
        StartCoroutine(GetCollider(playerInput, playerInputManager.playerCount - 1));
    }

    private void PreventOverlap() {
        ColliderDistance2D distance = Physics2D.Distance(playerColliders[0], playerColliders[1]);
        if (distance.isOverlapped) {
            playerSoftBodies[0].AddForce(-distance.normal * Mathf.Abs(distance.distance));
            playerSoftBodies[1].AddForce(distance.normal * Mathf.Abs(distance.distance));
        }
    }

    private IEnumerator GetCollider(PlayerInput playerInput, int playerIndex) {
        yield return null;
        playerColliders[playerIndex] = playerInput.GetComponentInChildren<PolygonCollider2D>();
    }
}
