using System;
using TMPro;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

[Serializable]
public class RoundState : GameBaseState
{
    [SerializeField] private float maxRoundTime;
    [SerializeField] private float currRoundTime;
    
    [SerializeField] private Transform[] spawns = new Transform[2];
    [SerializeField] private GameObject[] props;

    [SerializeField] private GameObject ball;
    
    [SerializeField] private CinemachineCamera cinemachineCamera;
    public override void EnterState(GameManager manager) {
        currRoundTime = maxRoundTime;
        MapManager.Instance.NextMap();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void UpdateState(GameManager manager) {
        currRoundTime -= Time.deltaTime;
        PointUI.Instance?.UpdateTimer(currRoundTime);
        if (currRoundTime <= 0) {
            // check if a player is in the lead, otherwise go into sudden death
            Debug.Log("SUDDEN DEATH");
            PointManager pointManager = PointManager.Instance;
            pointManager.suddenDeath = true;
            if (pointManager.points[0] == pointManager.points[1]) return;
            pointManager.RoundWon(pointManager.points[0] > pointManager.points[1] ? 0 : 1);
        }
    }

    public override void ExitState(GameManager manager) {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        PointManager.Instance.onScoreStart.RemoveListener(StartScoreCameraEffects);
        PointManager.Instance.onScoreEnd.RemoveListener(EndScoreCameraEffects);
        PointManager.Instance.suddenDeath = false;
    }

    private void ResetPlayerPos() {
        //each player goes back to spawn (move the soft bodies)
        for (int i = 0; i < spawns.Length; i++) {
            SoftBody softBody = Multiplayer.Instance.players[i].GetComponentInChildren<SoftBody>();
            softBody.MoveSoftBody(spawns[i].position);
            softBody.ResetVelocity();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        //get the spawns
        Transform spawnParent = GameObject.FindGameObjectWithTag("PlayerSpawns").transform;
        for (int i = 0; i < spawns.Length; i++) {
            spawns[i] = spawnParent.GetChild(i);
        }
        ResetPlayerPos();
        PointUI.Instance.UpdateRoundsWon();
        PointUI.Instance.UpdateAdvantage();
        PointUI.Instance.SetTimerMaxValue(maxRoundTime);
        SetUpProps();
        
        cinemachineCamera = Object.FindObjectOfType<CinemachineCamera>();

        PointManager.Instance.onScoreStart.AddListener(StartScoreCameraEffects);
        PointManager.Instance.onScoreEnd.AddListener(EndScoreCameraEffects);
        
        ball = GameObject.FindGameObjectWithTag("Ball");
    }

    private void SetUpProps() {
        GameObject[] propSpawns = GameObject.FindGameObjectsWithTag("PropSpawn");
        Transform propParent = GameObject.Find("Props").transform;
        foreach (GameObject spawn in propSpawns) {
            GameObject prop = props[Random.Range(0, props.Length)];
            if (prop == null) continue;
            Object.Instantiate(prop, spawn.transform.position, Quaternion.identity, propParent);
        }
    }

    private void StartScoreCameraEffects() {
        // cinemachineCamera.LookAt = ball.transform;
        cinemachineCamera.Follow = ball.transform;
        Time.timeScale = 0.5f;
    }

    private void EndScoreCameraEffects() {
        cinemachineCamera.Follow = null;
        cinemachineCamera.transform.position = new Vector3(0, 0, -10);
        Time.timeScale = 1;
    }
}
