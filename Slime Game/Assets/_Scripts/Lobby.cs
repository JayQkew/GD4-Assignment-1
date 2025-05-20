using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour
{
    private float currTime;
    private const float MaxTime = 4;

    private String startText;
    [SerializeField] private TextMeshProUGUI timeText;

    [SerializeField] private GameObject player1ReadyText;
    [SerializeField] private GameObject player2ReadyText;
    [SerializeField] private GameObject[] readyTxt;

    private void Start() {
        currTime = MaxTime;
        startText = timeText.text;
        // WinAllocation.Instance.CalculateWinner();
        // LevelList.instance.listIndex = 0;
        // LevelList.instance.team1Total = 0;
        // LevelList.instance.team2Total = 0;
    }

    private void Update() {
        if (Multiplayer.Instance.ready[0] && Multiplayer.Instance.ready[1]) UpdateTimer();
        else ResetTimer();

        readyTxt[0].SetActive(Multiplayer.Instance.ready[0]);
        readyTxt[1].SetActive(Multiplayer.Instance.ready[1]);
    }

    private void UpdateTimer() {
        currTime -= Time.deltaTime;
        if (currTime <= 0) SceneChangeManager.Instance.ChangeScene();
        else if (currTime <= 1) timeText.text = "GO!";
        else if (currTime <= 2) timeText.text = "1";
        else if (currTime <= 3) timeText.text = "2";
        else if (currTime <= 4) timeText.text = "3";
    }

    private void ResetTimer() {
        currTime = MaxTime;
        timeText.text = startText;
    }
}