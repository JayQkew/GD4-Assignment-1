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
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        if (sceneCount <= 1) return; // Prevent errors if there is only one scene.

        int newScene;
        do
        {
            newScene = UnityEngine.Random.Range(0, sceneCount);
        } while (newScene == SceneManager.GetActiveScene().buildIndex); // Ensure a different scene

        SceneManager.LoadScene(newScene);
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
