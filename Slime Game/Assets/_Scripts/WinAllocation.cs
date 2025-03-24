using UnityEngine;

public class WinAllocation : MonoBehaviour
{   
    [SerializeField]
    private GameObject player1;
    [SerializeField]
    private GameObject player2;
    void Start()
    {
        Debug.Log(LevelList.instance.WhoWon());
        if (LevelList.instance.WhoWon() == 0)
        {
            player1.SetActive(true);
            player2.SetActive(false);
        }
        else if (LevelList.instance.WhoWon() == 1)
        {
            player1.SetActive(false);
            player2.SetActive(true);
        }
        else if (LevelList.instance.WhoWon() == 3)
        {
            player1.SetActive(false);
            player2.SetActive(false);
        }
    }
}
