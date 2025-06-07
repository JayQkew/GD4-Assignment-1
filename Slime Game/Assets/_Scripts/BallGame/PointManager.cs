using System;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PointManager : MonoBehaviour
{
    public static PointManager Instance { get; private set; }

    [Header("Ball")] public Transform ballSpawn;
    public GameObject ball;

    [Header("Points")] 
    [SerializeField] private TextMeshProUGUI[] scoreText;

    [SerializeField] private int pointsToWinRound;
    [SerializeField] private int minRoundsToWin;
    [SerializeField] private int[] points = new int[2];
    [SerializeField] private int[] roundsWon = new int[2];

    private void Awake() {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) Score(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) Score(1);
    }

    // 0 index for playerScored
    public void Score(int playerScored) {
        points[playerScored]++;
        if (points[playerScored] >= pointsToWinRound) {
            Debug.Log($"Player {playerScored} wins the round!");
            RoundWon(playerScored);
        }
    }

    private void RoundWon(int playerScored) {
        roundsWon[playerScored]++;
        int otherPlayerRounds = roundsWon[(playerScored + 1) % 2];
        if (roundsWon[playerScored] >= otherPlayerRounds + 2) {
            Debug.Log($"Player {playerScored} wins the GAME!");

            //resets the rounds
            for (int i = 0; i < roundsWon[playerScored]; i++) {
                roundsWon[playerScored] = 0;
            }
        }

        //resets the points
        for (int i = 0; i < points.Length; i++) {
            points[i] = 0;
        }
    }

    public void RespawnBall() {
        ball.transform.position = ballSpawn.position;
        ball.GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        // get the score texts
        if (scene.name.Split('_')[0] == "Map") {
            scoreText[0] = GameObject.Find("Team 1 (Blue)").GetComponent<TextMeshProUGUI>();
            scoreText[1] = GameObject.Find("Team 1 (Pink)").GetComponent<TextMeshProUGUI>();
            ballSpawn = GameObject.Find("Ball Spawn").transform;
        }
    }
}

public enum Teams
{
    TeamOne = 0,
    TeamTwo = 1
}