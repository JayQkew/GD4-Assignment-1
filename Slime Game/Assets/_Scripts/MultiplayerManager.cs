using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class MultiplayerManager : MonoBehaviour
{
    private PlayerInputManager _playerInputManager;
    public int playerCount = 0;
    public GameObject bigSlime;

    [SerializeField] private String[] _layers;
    [SerializeField] private Material[] _materials;
    
    [SerializeField] private Movement[] _playerMovements = new Movement[2];

    private void Awake()
    {
        _playerInputManager = GetComponent<PlayerInputManager>();
    }

    private void Update()
    {
        if (_playerMovements[0]&& _playerMovements[1])
        {
            if(_playerMovements[0].isBigSlime && _playerMovements[1].isBigSlime) Debug.Log("Both Players Grabbing");
        }
    }

    private void BigSlimeMerge()
    {
        //find the middle between the slimes
        Vector2 midPoint = (_playerMovements[0].transform.position - _playerMovements[1].transform.position)/2;
        bigSlime.transform.position = midPoint;
        bigSlime.SetActive(true);
        
        _playerMovements[0].gameObject.SetActive(false);
        _playerMovements[1].gameObject.SetActive(false);
    }

    private void BigSlimeSplit()
    {
        
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