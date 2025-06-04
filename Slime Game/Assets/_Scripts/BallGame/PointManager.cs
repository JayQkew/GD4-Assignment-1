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

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
                MapManager.instance.UpdateTotalScores(scoredAgainst);
                SceneChangeManager.Instance.ChangeScene();
            }
        }
        else
        {
            if (team1Score == maximumScore)
            {
                MapManager.instance.UpdateTotalScores(scoredAgainst);
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