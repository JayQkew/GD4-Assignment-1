using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelList : MonoBehaviour
{
    private int previousLevel;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
        }
    }
    public static LevelList instance { get; private set; }

    [SerializeField]
    public  List<int> list = new List<int>(3);

    public void AddSceneToList(int scene)
    {
        // Add the build index of the active scene to the list.
        list.Add(scene);
        Debug.Log(list);
    }

    public bool CheckLevel(int targetLevel)
    {
        // Check if the target level (scene build index) exists in the list.
        if (list != null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == targetLevel)
                {
                    return false;
                }
            }
        }

        return true;
    }
}