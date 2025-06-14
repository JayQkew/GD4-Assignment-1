using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BombSpawner : MonoBehaviour
{
    [Header("Bombs")] public GameObject bombPrefab;
    [SerializeField] private int maxLiveBombs;
    [SerializeField] private float spawnForce;
    [SerializeField] private List<GameObject> bombs;
    private int liveBombs;

    [Header("Timer")] [SerializeField] private float currTime;
    [SerializeField] private float spawnTime;

    private void Start() {
        for (int i = 0; i < maxLiveBombs; i++) {
            GameObject bomb = Instantiate(bombPrefab, transform);
            bombs.Add(bomb);
            // bomb.SetActive(false);
        }
    }

    private void Update() {
        currTime += Time.deltaTime;
        if (currTime >= spawnTime) {
            SpawnBomb();
            currTime = 0;
        }
    }

    private void SpawnBomb() {
        if (LiveBombs() < maxLiveBombs) {
            float randAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(Mathf.Cos(randAngle), Mathf.Sin(randAngle));

            GameObject bomb = FetchBomb();
            bomb.GetComponent<Rigidbody2D>().AddForce(dir * spawnForce, ForceMode2D.Impulse);
        }
    }

    private int LiveBombs() {
        int count = 0;
        foreach (GameObject bomb in bombs) {
            count += bomb.activeSelf ? 1 : 0;
        }

        return count;
    }

    private GameObject FetchBomb() {
        foreach (GameObject bomb in bombs) {
            if (bomb.activeSelf == false) {
                bomb.SetActive(true);
                bomb.transform.position = transform.position;
                return bomb;
            }
        }

        return null;
    }
}