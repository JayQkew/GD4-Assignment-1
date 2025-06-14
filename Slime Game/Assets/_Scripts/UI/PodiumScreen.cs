using UnityEngine;
using UnityEngine.SceneManagement;

public class PodiumScreen : MonoBehaviour
{
    public void StartScreen() {
        GameManager.Instance.SwitchState(GameState.Limbo);
    }
}
