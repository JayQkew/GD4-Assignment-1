using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PointUI : MonoBehaviour
{
    public static PointUI Instance { get; private set; }
    [SerializeField] private GameObject[] advantage = new GameObject[2];
    [SerializeField] private TextMeshProUGUI[] roundsWon = new TextMeshProUGUI[2];
    [SerializeField] private Transform[] playerPoints = new Transform[2];
    [SerializeField] private Slider[] timerSlider = new Slider[2];
    private Image[,] _points = new Image[2,3];

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        GetPoints();
    }

    private void GetPoints() {
        for (int i = 0; i < playerPoints.Length; i++) {
            int children = playerPoints[i].childCount;
            for (int j = 0; j < children; j++) {
                _points[i,j] = playerPoints[i].GetChild(j).GetComponent<Image>();
            }
        }
    }

    public void UpdatePointsUI(int player, int points) {
        for (int i = 0; i < points; i++) {
            Color color = playerPoints[player].GetChild(0).GetComponent<Image>().color;
            color.a = 1;
            _points[player,i].color = color;
        }
    }

    public void UpdateRoundsWon() {
        roundsWon[0].text = PointManager.Instance.roundsWon[0].ToString();
        roundsWon[1].text = PointManager.Instance.roundsWon[1].ToString();
    }

    public void UpdateAdvantage() {
        //check who has advantage
        int advantageTo = PointManager.Instance.Advantage();
        Debug.Log("Advantage to " + advantageTo);
        if (advantageTo == -1) {
            advantage[0].SetActive(false);
            advantage[1].SetActive(false);
        }
        else if (advantageTo == 2 || advantageTo == 3) {
            advantage[0].SetActive(advantageTo % 2 == 0);
            advantage[1].SetActive(advantageTo % 2 == 1);

            advantage[advantageTo % 2].GetComponent<TextMeshProUGUI>().text = "RP";
        }
        else {
            advantage[advantageTo].SetActive(true);
            advantage[(advantageTo + 1) % 2].SetActive(false);
            
            advantage[advantageTo].GetComponent<TextMeshProUGUI>().text = "AD";
        }
    }

    public void UpdateTimer(float max, float curr) {
        timerSlider[0].maxValue = max;
        timerSlider[1].maxValue = max;
        
        timerSlider[0].value = curr;
        timerSlider[1].value = curr;
    }
}
