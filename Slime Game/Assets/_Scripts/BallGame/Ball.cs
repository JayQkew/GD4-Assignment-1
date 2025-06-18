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
        PointManager.Instance.onScoreEnd.AddListener(Respawn);
    }

    private void OnDestroy() {
        PointManager.Instance.onScoreEnd.RemoveListener(Respawn);
        SceneManager.sceneLoaded -= OnSceneLoaded;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.ballHitAudioSource = GetComponent<AudioSource>();
            AudioManager.Instance.BallHitMusic();
        }
    }
}