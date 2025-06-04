using UnityEngine;

public class WinAllocation : MonoBehaviour
{   
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }
    
    public static WinAllocation Instance {get; private set;}
    
    [SerializeField]
    private GameObject player1;
    [SerializeField]
    private GameObject player2;
    public void CalculateWinner()
    {
        Debug.Log(MapManager.instance.WhoWon());
        if (MapManager.instance.WhoWon() == 0)
        {
            player1.SetActive(true);
            player2.SetActive(false);
        }
        else if (MapManager.instance.WhoWon() == 1)
        {
            player1.SetActive(false);
            player2.SetActive(true);
        }
        else if (MapManager.instance.WhoWon() == 3)
        {
            player1.SetActive(false);
            player2.SetActive(false);
        }
    }
}
