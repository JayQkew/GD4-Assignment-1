using UnityEngine;
using UnityEngine.SceneManagement;

public class LimboState : GameBaseState
{
    public override void EnterState(GameManager manager) {
        SceneManager.LoadScene("StartScreen");
        Object.Destroy(Multiplayer.Instance.gameObject);
    }

    public override void UpdateState(GameManager manager) {
    }

    public override void ExitState(GameManager manager) {
    }
}
