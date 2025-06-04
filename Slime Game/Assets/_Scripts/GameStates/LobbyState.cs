using System;
using TMPro;
using UnityEngine;

[Serializable]
public class LobbyState : GameBaseState
{
    private float currTime;
    private const float MaxTime = 4;

    private String startText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private GameObject[] readyTxt;
    public override void EnterState(GameManager manager) {
        currTime = MaxTime;
        startText = timeText.text;
    }

    public override void UpdateState(GameManager manager) {
        bool[] playerReady = Multiplayer.Instance.ready;
        if (playerReady[0] && playerReady[1]) UpdateTimer();
        else ResetTimer();
        
        readyTxt[0].SetActive(playerReady[0]);
        readyTxt[1].SetActive(playerReady[1]);
    }

    public override void ExitState(GameManager manager) {
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
