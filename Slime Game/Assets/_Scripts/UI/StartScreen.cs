using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    public void GoToHowTo() {
        GameManager.Instance.SwitchState(GameState.HowTo);
    }

    public void ExitGame() {
        Application.Quit();
    }
    
    public void GoToHowToPlay() => SceneManager.LoadScene("HowToPlay");
}
