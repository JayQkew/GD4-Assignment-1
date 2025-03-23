using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class MultiplayerManager : MonoBehaviour
{
    public static MultiplayerManager Instance {get; private set;}
    private PlayerInputManager _playerInputManager;
    public int playerCount = 0;

    [SerializeField] private String[] _layers;
    [SerializeField] private Material[] _materials;
    [SerializeField] private GameObject[] _faces;

    [SerializeField] private Movement[] _playerMovements = new Movement[2];
    public List<PlayerInput> players = new List<PlayerInput>();
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
        _playerInputManager = GetComponent<PlayerInputManager>();
    }

    public void RegisterPlayer(PlayerInput playerInput)
    {
        playerCount++;
        players.Add(playerInput);
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
        Instantiate(_faces[playerCount], softbody.transform);

        Debug.Log("Joined");
        playerCount++;
    }
}