using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerInputManager))]
public class MultiplayerManager : MonoBehaviour
{
    public static MultiplayerManager Instance {get; private set;}
    private PlayerInputManager _playerInputManager;
    public int playerCount = 0;

    [SerializeField] private String[] _layers;
    [SerializeField] private Material[] _materials;

    [SerializeField] private Movement[] _playerMovements = new Movement[2];
    [SerializeField] private PolygonCollider2D[] _playerColliders = new PolygonCollider2D[2];
    [SerializeField] private Transform[] _playerSpawns = new Transform[2];
    public List<PlayerInput> players = new List<PlayerInput>();
    public bool[] readyStates = new bool[2];
    private Collider2D[] results = new Collider2D[10];
    
    private ContactFilter2D _contactFilter;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(this);
        }
        _playerInputManager = GetComponent<PlayerInputManager>();
        _contactFilter = new ContactFilter2D();
        _contactFilter.useTriggers = false;
        _contactFilter.maxDepth = -20f;
        _contactFilter.minDepth = -22f;
        _contactFilter.useOutsideDepth = false;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // grab the spawners
        // make players spawn at designated spawners
        _playerSpawns[0] = null;
        _playerSpawns[1] = null;

        Transform playerSpawn = GameObject.FindGameObjectWithTag("PlayerSpawns").transform;
        for (int i = 0; i < _playerSpawns.Length; i++)
        {
            _playerSpawns[i] = playerSpawn.GetChild(i);
        }

        if (SceneManager.GetActiveScene().name != "Lobby")
        {
            Debug.Log("Move slime");
            for (int i = 0; i < players.Count; i++)
            {
                players[i].GetComponentInChildren<SoftBody>().MoveSlime(_playerSpawns[i].position);
            }
        }
    }

    private void Update()
    {
        if(playerCount == 2) CheckOverlap();
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        GameObject softbody = playerInput.transform.GetChild(0).gameObject;

        _playerMovements[playerCount] = playerInput.GetComponentInChildren<Movement>();
        playerInput.gameObject.name = "Player" + (playerCount + 1);
        playerInput.transform.SetParent(transform);
        playerInput.transform.position = _playerSpawns[playerCount].position;
        softbody.layer = LayerMask.NameToLayer(_layers[playerCount]);
        softbody.transform.position = Vector3.zero;
        softbody.GetComponent<SoftBody>().meshMaterial = _materials[playerCount];
        players.Add(playerInput);
        
        
        playerInput.actions["Ready"].performed += ctx => SetReady(playerInput.playerIndex);
        Debug.Log("Joined");
        playerCount++;
    }

    private void CheckOverlap()
    {
        _playerColliders[0] = _playerMovements[0].GetComponent<PolygonCollider2D>();
        _playerColliders[1] = _playerMovements[1].GetComponent<PolygonCollider2D>();

        Collider2D[] test = results;
        
        ColliderDistance2D distance = Physics2D.Distance(_playerColliders[0], _playerColliders[1]);
        if (distance.isOverlapped)
        {
            Debug.Log("Overlap");
            _playerColliders[0].GetComponent<SoftBody>().AddForce(-distance.normal * Mathf.Abs(distance.distance));
            _playerColliders[1].GetComponent<SoftBody>().AddForce(distance.normal * Mathf.Abs(distance.distance));
            //WORKS
        }
    }
    
    private void SetReady(int playerIndex)
    {
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            if (playerIndex >= readyStates.Length) return;

            readyStates[playerIndex] = !readyStates[playerIndex];
            Debug.Log("Player " + (playerIndex + 1) + " is ready!");
        }
    }
}