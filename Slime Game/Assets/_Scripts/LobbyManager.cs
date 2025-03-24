using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public float currTime;
    public float maxTime;

    private String startText;
    
    [SerializeField] private TextMeshProUGUI timeText;
    
    [SerializeField] private GameObject player1ReadyText;
    [SerializeField] private GameObject player2ReadyText;

    private void Start()
    {
        currTime = maxTime;
        startText = timeText.text;
        WinAllocation.Instance.CalculateWinner();
        LevelList.instance.listIndex = 0;
        LevelList.instance.team1Total = 0;
        LevelList.instance.team2Total = 0;
    }

    private void Update()
    {
        if (MultiplayerManager.Instance.readyStates[0] && MultiplayerManager.Instance.readyStates[1])
        {
            UpdateTimer();
        }
        else
        {
            ResetTimer();
        }
        
        player1ReadyText.SetActive(MultiplayerManager.Instance.readyStates[0]);
        player2ReadyText.SetActive(MultiplayerManager.Instance.readyStates[1]);
    }

    private void UpdateTimer()
    {
        currTime -= Time.deltaTime;
        if (currTime <= 0) SceneChangeManager.Instance.ChangeScene();
        else if (currTime <= 1) timeText.text = "GO!";
        else if (currTime <= 2) timeText.text = "1";
        else if (currTime <= 3) timeText.text = "2";
        else if (currTime <= 4) timeText.text = "3";
        
    }

    private void ResetTimer()
    {
        currTime = maxTime;
        
        timeText.text = startText;
    }
}
