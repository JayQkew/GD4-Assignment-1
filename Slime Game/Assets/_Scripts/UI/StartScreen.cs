using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    public void GoToMapSelect() {
        SceneManager.LoadScene("SelectMap");
    }

    public void ExitGame() {
        Application.Quit();
    }
}
