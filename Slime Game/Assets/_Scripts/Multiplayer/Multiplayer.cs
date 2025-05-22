using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Multiplayer : MonoBehaviour
{
    public static Multiplayer Instance { get; private set; }
    private PlayerInputManager _playerInputManager;
    public GameObject[] players = new GameObject[2];
    public bool[] ready = new bool[2];

    [Header("Soft Body")]
    [SerializeField] private String[] layers;
    [SerializeField] private Material[] materials;
    
    [Header("Player")]
    [SerializeField] private Transform[] spawnPoints;
    
    private MultiplayerCollider _multiplayerCollider;
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else {
            Destroy(this);
        }
        
        _playerInputManager = GetComponent<PlayerInputManager>();
        _multiplayerCollider = GetComponent<MultiplayerCollider>();
    }

    public void OnPlayerJoined(PlayerInput playerInput) {
        SetSoftBody(playerInput);
        SetPlayer(playerInput);
        
        players[_playerInputManager.playerCount - 1] = playerInput.gameObject;

        // playerInput.actions["Ready"].performed += ctx => SetReady(playerInput.playerIndex);
        _multiplayerCollider.OnPlayerJoined(playerInput);
    }

    private void SetSoftBody(PlayerInput playerInput) {
        GameObject softBody = playerInput.transform.GetChild(0).gameObject;
        softBody.transform.position = Vector3.zero;
        softBody.layer = LayerMask.NameToLayer(layers[_playerInputManager.playerCount - 1]);
        softBody.GetComponent<SoftBody>().meshMaterial = materials[_playerInputManager.playerCount - 1];
    }

    private void SetPlayer(PlayerInput playerInput) {
        playerInput.gameObject.name = $"Player {_playerInputManager.playerCount}";
        playerInput.transform.SetParent(transform);
        playerInput.transform.position = spawnPoints[_playerInputManager.playerCount - 1].position;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        spawnPoints = new Transform[2];
        Transform playerSpawnPoints = GameObject.FindGameObjectWithTag("PlayerSpawns").transform;
        for (int i = 0; i < spawnPoints.Length; i++) {
            spawnPoints[i] = playerSpawnPoints.transform;
        }
    }
    
    private void SetReady(int playerIndex) {
        if (SceneManager.GetActiveScene().name == "Lobby") {
            if (playerIndex >= ready.Length - 1) return;
            ready[playerIndex] = !ready[playerIndex];
        }
    }
}
