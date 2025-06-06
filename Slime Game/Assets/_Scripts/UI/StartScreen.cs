using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    public void GoToMapSelect() {
        GameManager.Instance.SwitchState(GameState.MapSelect);
    }

    public void ExitGame() {
        Application.Quit();
    }
}
