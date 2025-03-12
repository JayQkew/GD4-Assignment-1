using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class MultiplayerManager : MonoBehaviour
{
    private PlayerInputManager _playerInputManager;
    public int playerCount = 0;

    [SerializeField] private String[] _layers;
    [SerializeField] private Material[] _materials;

    [SerializeField] private Movement[] _playerMovements = new Movement[2];
    
    private void Awake()
    {
        _playerInputManager = GetComponent<PlayerInputManager>();
    }
    
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        GameObject softbody = playerInput.transform.GetChild(0).gameObject;

        _playerMovements[playerCount] = playerInput.GetComponentInChildren<Movement>();
        playerInput.gameObject.name = "Player" + (playerCount + 1);
        playerInput.transform.SetParent(transform);
        softbody.layer = LayerMask.NameToLayer(_layers[playerCount]);
        softbody.transform.position = Vector3.zero;
        softbody.GetComponent<SoftBody>().meshMaterial = _materials[playerCount];

        Debug.Log("Joined");
        playerCount++;
    }
}