using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class LobbyState : GameBaseState
{
    private float currTime;
    private const float MaxTime = 4;

    private String startText = "Press X / A to Join\n\nTriangle / Y to ready Up!";
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private GameObject[] readyTxt;
    public override void EnterState(GameManager manager) {
        SceneManager.LoadScene("Lobby 2.0");
        SceneManager.sceneLoaded += OnSceneLoaded;
        currTime = MaxTime;
    }

    public override void UpdateState(GameManager manager) {
        bool[] playerReady = Multiplayer.Instance.ready;
        if (playerReady[0] && playerReady[1]) UpdateTimer(manager);
        else ResetTimer();
        
        readyTxt[0].SetActive(playerReady[0]);
        readyTxt[1].SetActive(playerReady[1]);
    }

    public override void ExitState(GameManager manager) {
    }
    
    private void UpdateTimer(GameManager manager) {
        currTime -= Time.deltaTime;
        if (currTime <= 0) {
            MapManager.Instance.NextMap();
            manager.SwitchState(GameState.Match);
        }
        else if (currTime <= 1) timeText.text = "GO!";
        else if (currTime <= 2) timeText.text = "1";
        else if (currTime <= 3) timeText.text = "2";
        else if (currTime <= 4) timeText.text = "3";
    }
    
    private void ResetTimer() {
        currTime = MaxTime;
        timeText.text = startText;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        timeText = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
        startText = timeText.text;
    }
}
