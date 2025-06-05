using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameState state;

    private GameBaseState currState;
    public MapSelectState mapSelectState = new MapSelectState();
    public LobbyState lobbyState = new LobbyState();
    public MatchState matchState = new MatchState();
    public DraftState draftState = new DraftState();
    public PodiumState podiumState = new PodiumState();
    public LimboState limboState = new LimboState();

    private void Start() {
        currState = lobbyState;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) SwitchState(GameState.MapSelect);
        currState.UpdateState(this);
    }

    public void SwitchState(GameState newState) {
        currState.ExitState(this);
        switch (newState) {
            case GameState.MapSelect:
                currState = mapSelectState;
                break;
            case GameState.Lobby:
                currState = lobbyState;
                break;
            case GameState.Match:
                currState = matchState;
                break;
            case GameState.Draft:
                currState = draftState;
                break;
            case GameState.Winner:
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
    MapSelect,
    Lobby,
    Match,
    Draft,
    Winner,
    Limbo
}