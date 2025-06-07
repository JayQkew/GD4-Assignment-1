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

    [Header("Points")] [SerializeField] private TextMeshProUGUI text_team1Score;
    [SerializeField] private TextMeshProUGUI text_team2Score;
    [Space(10)] public int team1Score;
    public int team2Score;
    [Space(5)] [SerializeField] private int maximumScore; //Maximum score allowed before a team wins the round

    [SerializeField] private int minWinPoints;
    [SerializeField] private int roundWinPoints;
    [SerializeField] private int[] points = new int[2];
    [SerializeField] private int[] roundsWon = new int[2];
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) Score(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) Score(1);
    }

    // 0 index for playerScored
    public void Score(int playerScored) {
        points[playerScored]++;
        int otherPlayerPoints = points[(playerScored + 1) % 2];
        if (points[playerScored] > minWinPoints && points[playerScored] >= otherPlayerPoints + 2) {
            Debug.Log($"Player {playerScored} wins the round!");
            RoundWon(playerScored);
        }
    }

    private void RoundWon(int playerScored) {
        roundsWon[playerScored]++;
        int otherPlayerRounds = roundsWon[(playerScored + 1) % 2];
        if(roundsWon[playerScored] >= otherPlayerRounds + 2) {
            Debug.Log($"Player {playerScored} wins the GAME!");
        }

        //resets the points
        for (int i = 0; i < points.Length; i++) {
            points[i] = 0;
        }
    }

    public void UpdateScore(Teams scoredAgainst)
    {
        if (scoredAgainst == Teams.TeamOne)
        {
            team2Score++;
            text_team2Score.text = team2Score.ToString();
        }
        else
        {
            team1Score++;
            text_team1Score.text = team1Score.ToString();
        }
    }

    public void RespawnBall()
    {
        ball.transform.position = ballSpawn.position;
        ball.GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
    }

    public void CheckScores(Teams scoredAgainst)
    {
        if (scoredAgainst == Teams.TeamOne)
        {
            if (team2Score == maximumScore)
            {
                MapManager.Instance.UpdateTotalScores(scoredAgainst);
                SceneChangeManager.Instance.ChangeScene();
            }
        }
        else
        {
            if (team1Score == maximumScore)
            {
                MapManager.Instance.UpdateTotalScores(scoredAgainst);
                SceneChangeManager.Instance.ChangeScene();
            }
        }
    }
}

public enum Teams
{
    TeamOne = 0,
    TeamTwo = 1
}