using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PointManager : MonoBehaviour
{
    public static PointManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI[] scoreText = new TextMeshProUGUI[2];
    [SerializeField] private int pointsToWinRound;
    [SerializeField] private int minRoundsToWin;
    public int[] points = new int[2];
    public int[] roundsWon = new int[2];
    public bool suddenDeath;

    public UnityEvent onScore;

    private void Awake() {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) Score(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) Score(1);
    }

    // 0 index for playerScored
    public void Score(int playerScored) {
        points[playerScored]++;
        scoreText[playerScored].text = points[playerScored].ToString();
        onScore?.Invoke();
        if (points[playerScored] >= pointsToWinRound || suddenDeath) {
            Debug.Log($"Player {playerScored} wins the round!");
            RoundWon(playerScored);
        }
    }

    public void RoundWon(int playerScored) {
        roundsWon[playerScored]++;
        int otherPlayerRounds = roundsWon[(playerScored + 1) % 2];
        
        //resets the points
        for (int i = 0; i < points.Length; i++) {
            points[i] = 0;
            scoreText[i].text = points[i].ToString();
        }
        
        if (roundsWon[playerScored] >= otherPlayerRounds + 2) {
            for (int i = 0; i < roundsWon.Length; i++) {
                roundsWon[i] = 0;
                scoreText[i].text = points[i].ToString();
            }
            Debug.Log($"Player {playerScored} wins the GAME!");
            GameManager.Instance.podiumState.winnerNumber = playerScored;
            GameManager.Instance.SwitchState(GameState.Podium);
        }
        else {
            Deck lostPlayerDeck = Multiplayer.Instance.players[(playerScored + 1) % 2].transform.GetChild(0).GetComponent<Deck>();
            GameManager.Instance.draftState.lostPlayerDeck = lostPlayerDeck;
            GameManager.Instance.SwitchState(GameState.Draft);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        // get the score texts
        if (scene.name.Split('_')[0] == "Map") {
            scoreText[0] = GameObject.Find("Player 1 Score").GetComponent<TextMeshProUGUI>();
            scoreText[1] = GameObject.Find("Player 2 Score").GetComponent<TextMeshProUGUI>();
        }
    }
}

public enum Teams
{
    TeamOne = 0,
    TeamTwo = 1
}