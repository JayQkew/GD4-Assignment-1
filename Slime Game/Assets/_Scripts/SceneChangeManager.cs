using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    public void ChangeScene()
    {
        int newScene = UnityEngine.Random.Range(SceneManager.sceneCount - 1, 0);
        LoadNewScene(newScene);
    }

    private void LoadNewScene(int newScene)
    {
        if (newScene != SceneManager.GetActiveScene().buildIndex)
        {
            SceneManager.LoadScene(newScene);
        }
        else
        {
            int nextNewScene = UnityEngine.Random.Range(SceneManager.sceneCount - 1, 0);
            LoadNewScene(nextNewScene);
        }    
    }
}
