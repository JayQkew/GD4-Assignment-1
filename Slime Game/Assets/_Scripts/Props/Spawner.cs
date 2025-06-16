using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [Header("Bombs")] 
    public GameObject spawnPrefab;
    [SerializeField] private int maxLiveSpawns;
    [SerializeField] private float spawnForce;
    [SerializeField] private List<GameObject> spawns;
    private int liveSpawns;

    [Header("Timer")] 
    [SerializeField] private float currTime;
    [SerializeField] private float spawnTime;

    private void Start() {
        for (int i = 0; i < maxLiveSpawns; i++) {
            GameObject spawn = Instantiate(spawnPrefab, transform);
            spawns.Add(spawn);
        }
    }

    private void Update() {
        currTime += Time.deltaTime;
        if (currTime >= spawnTime) {
            Spawn();
            currTime = 0;
        }
    }

    private void Spawn() {
        if (LiveSpawns() < maxLiveSpawns) {
            float randAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(Mathf.Cos(randAngle), Mathf.Sin(randAngle));

            GameObject spawn = FetchSpawn();
            spawn.GetComponent<Rigidbody2D>().AddForce(dir * spawnForce, ForceMode2D.Impulse);
        }
    }

    private int LiveSpawns() {
        int count = 0;
        foreach (GameObject s in spawns) {
            count += s.activeSelf ? 1 : 0;
        }

        return count;
    }

    private GameObject FetchSpawn() {
        foreach (GameObject s in spawns) {
            if (s.activeSelf == false) {
                s.SetActive(true);
                s.transform.position = transform.position;
                return s;
            }
        }

        return null;
    }
}