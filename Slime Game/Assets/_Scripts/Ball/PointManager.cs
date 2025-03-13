using System;
using TMPro;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    public static PointManager Instance {get; private set;}
    [SerializeField] private TextMeshProUGUI text_player1Score;
    [SerializeField] private TextMeshProUGUI text_player2Score;
    
    public int player1Score;
    public int player2Score;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateScore(int player)
    {
        if (player == 0)
        {
            player2Score++;
            text_player2Score.text = player2Score.ToString();
        }
        else
        {
            player1Score++;
            text_player1Score.text = player1Score.ToString();
        }
    }
}
