using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class DraftState : GameBaseState
{
    public override void EnterState(GameManager manager) {
        SceneManager.LoadScene("Draft");
        //disable the other players ui selection
        //draft cards
        //show current players cards
    }

    public override void UpdateState(GameManager manager) {
    }

    public override void ExitState(GameManager manager) {
    }
}
