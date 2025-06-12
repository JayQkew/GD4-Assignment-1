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
    private Image[,] points = new Image[2,3];

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
                points[i,j] = playerPoints[i].GetChild(j).GetComponent<Image>();
            }
        }
    }
}
