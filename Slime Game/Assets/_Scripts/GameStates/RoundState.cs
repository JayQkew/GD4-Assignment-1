using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class RoundState : GameBaseState
{
    [SerializeField] private float maxRoundTime;
    [SerializeField] private float currRoundTime;
    
    [SerializeField] private Transform[] spawns = new Transform[2];
    public override void EnterState(GameManager manager) {
        currRoundTime = maxRoundTime;
        MapManager.Instance.NextMap();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void UpdateState(GameManager manager) {
        currRoundTime -= Time.deltaTime;
        PointUI.Instance.UpdateTimer(maxRoundTime, currRoundTime);
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
    }
}
