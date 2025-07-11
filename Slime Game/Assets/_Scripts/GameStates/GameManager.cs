using System;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState state;

    private GameBaseState currState;
    public HowTo howTo = new HowTo();
    public MapSelectState mapSelectState = new MapSelectState();
    public LobbyState lobbyState = new LobbyState();
    public RoundState roundState = new RoundState();
    public DraftState draftState = new DraftState();
    public PodiumState podiumState = new PodiumState();
    public LimboState limboState = new LimboState();

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        currState = limboState;
    }

    private void Update() {
        currState.UpdateState(this);
    }

    public void SwitchState(GameState newState) {
        currState.ExitState(this);
        switch (newState) {
            case GameState.HowTo:
                currState = howTo;
                break;
            case GameState.MapSelect:
                currState = mapSelectState;
                break;
            case GameState.Lobby:
                currState = lobbyState;
                break;
            case GameState.Round:
                currState = roundState;
                break;
            case GameState.Draft:
                currState = draftState;
                break;
            case GameState.Podium:
                currState = podiumState;
                break;
            case GameState.Limbo:
                currState = limboState;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        currState.EnterState(this);
        state = newState;
    }
}

public abstract class GameBaseState
{
    public abstract void EnterState(GameManager manager);
    public abstract void UpdateState(GameManager manager);
    public abstract void ExitState(GameManager manager);
}

public enum GameState
{
    HowTo,
    MapSelect,
    Lobby,
    Round,
    Draft,
    Podium,
    Limbo
}