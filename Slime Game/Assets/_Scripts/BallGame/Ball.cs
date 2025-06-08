using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    private Rigidbody2D _rb;

    private void Awake() {
        SceneManager.sceneLoaded += OnSceneLoaded;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        PointManager.Instance.onScore.AddListener(Respawn);
    }

    private void OnDestroy() {
        PointManager.Instance.onScore.RemoveListener(Respawn);
    }

    private void Respawn() {
        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0f;
        _rb.rotation = 0f;
        transform.position = spawnPoint.position;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        spawnPoint = GameObject.Find("Ball Spawn").transform;
    }
}