using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class PodiumState : GameBaseState
{
    public int winnerNumber;
    public TextMeshProUGUI winnerText;
    [SerializeField] private Color[] podiumColors;
    public override void EnterState(GameManager manager) {
        SceneManager.LoadScene("Podium");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void UpdateState(GameManager manager) {
    }

    public override void ExitState(GameManager manager) {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        //get text
        winnerText = GameObject.Find("WinnerText").GetComponent<TextMeshProUGUI>();
        winnerText.color = podiumColors[winnerNumber];
        winnerText.text = $"Player  {winnerNumber + 1}  Wins!";
    }
}
