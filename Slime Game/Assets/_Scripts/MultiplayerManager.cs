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

    private void Awake()
    {
        _playerInputManager = GetComponent<PlayerInputManager>();
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        playerInput.transform.position = Vector3.zero;
        playerInput.gameObject.transform.parent.gameObject.name = "Player" + playerCount + 1;
        playerInput.gameObject.layer = LayerMask.NameToLayer(_layers[playerCount]);
        playerInput.GetComponent<SoftBody>().meshMaterial = _materials[playerCount];
        // playerInput.transform.SetParent(transform);
        
        Debug.Log("Joined");
        playerCount++;
    }
}