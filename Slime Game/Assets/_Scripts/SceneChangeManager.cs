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
        int newScene = LevelList.instance.SelectLevel();
        LevelList.instance.listIndex++;

        SceneManager.LoadScene(newScene);
    }
}
