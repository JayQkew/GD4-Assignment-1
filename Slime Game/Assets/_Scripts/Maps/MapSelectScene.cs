using UnityEngine;

public class MapSelectScene : MonoBehaviour
{
    public void SelectAllMaps() {
        GameManager.Instance.mapSelectState.SelectAllMaps();
    }
    public void NextScene() => GameManager.Instance.SwitchState(GameState.Lobby);
}
