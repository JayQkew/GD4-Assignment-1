using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameState state;

    public GameBaseState currState;
    
    public MapSelectState mapSelectState = new MapSelectState();
    public LobbyState lobbyState = new LobbyState();
    public MatchState matchState = new MatchState();
    public DraftState draftState = new DraftState();
    public PodiumState podiumState = new PodiumState();

    private void Start() {
        currState = mapSelectState;
    }

    private void Update() {
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
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        currState.EnterState(this);
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
    Winner
}