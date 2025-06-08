using System;
using UnityEngine;

[Serializable]
public class RoundState : GameBaseState
{
    [SerializeField] private float maxRoundTime;
    [SerializeField] private float currRoundTime;
    public override void EnterState(GameManager manager) {
        currRoundTime = 0f;
    }

    public override void UpdateState(GameManager manager) {
        currRoundTime += Time.deltaTime;
        if (currRoundTime >= maxRoundTime) {
            // check if a player is in the lead, otherwise go into sudden death
        }
    }

    public override void ExitState(GameManager manager) {
    }
}
