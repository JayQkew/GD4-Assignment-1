using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }
    
    public static SceneChangeManager Instance {get; private set;}
    public void ChangeScene()
    {
        int newScene = 0;
        if (LevelList.instance.listIndex < LevelList.instance.roundLimit)
        {
            newScene = LevelList.instance.SelectLevel();
            LevelList.instance.listIndex++;
        }
        else
        {
            newScene = 0;
        }

        SceneManager.LoadScene(newScene);
    }
}
