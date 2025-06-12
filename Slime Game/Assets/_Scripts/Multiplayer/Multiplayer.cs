using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Multiplayer : MonoBehaviour
{
    public static Multiplayer Instance { get; private set; }
    private PlayerInputManager _playerInputManager; 
    public PlayerInput[] playerInputs = new PlayerInput[2];
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
        playerInputs[_playerInputManager.playerCount - 1] = playerInput;

        playerInput.actions["Ready"].performed += ctx => SetReady(playerInput.playerIndex);
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
        if (scene.name != "Draft" && scene.name != "StartScreen" && scene.name != "MapSelect" && scene.name != "Podium") {
            spawnPoints = new Transform[2];
            Transform playerSpawnPoints = GameObject.FindGameObjectWithTag("PlayerSpawns").transform;
            for (int i = 0; i < spawnPoints.Length; i++) {
                spawnPoints[i] = playerSpawnPoints.GetChild(i);
            }
        }
    }

    public void SetUIInteraction(int player, bool active) {
        PlayerInput playerInput = playerInputs[player];
        EventSystem.current.GetComponent<InputSystemUIInputModule>().actionsAsset = playerInput.actions;
    
        Debug.Log(playerInput);
        if (active) {
            // Enable UI actions
            playerInput.actions["Navigate"].Enable();
            playerInput.actions["Submit"].Enable();
            playerInput.actions["Cancel"].Enable();
            playerInput.actions["Point"].Enable();
            playerInput.actions["Click"].Enable();
            playerInput.actions["ScrollWheel"].Enable();
            playerInput.actions["MiddleClick"].Enable();
            playerInput.actions["RightClick"].Enable();
        } else {
            // Disable UI actions
            playerInput.actions["Navigate"].Disable();
            playerInput.actions["Submit"].Disable();
            playerInput.actions["Cancel"].Disable();
            playerInput.actions["Point"].Disable();
            playerInput.actions["Click"].Disable();
            playerInput.actions["ScrollWheel"].Disable();
            playerInput.actions["MiddleClick"].Disable();
            playerInput.actions["RightClick"].Disable();
        }
    }
    
    private void SetReady(int playerIndex) {
        if (SceneManager.GetActiveScene().name == "Lobby" +
            "") {
            ready[playerIndex] = !ready[playerIndex];
        }
    }
}
