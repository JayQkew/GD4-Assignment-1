using UnityEngine;
using UnityEngine.SceneManagement;

public class HowTo : GameBaseState
{
    public override void EnterState(GameManager manager) {
        SceneManager.LoadScene("HowToPlay");
    }

    public override void UpdateState(GameManager manager) {
    }

    public override void ExitState(GameManager manager) {
    }
}