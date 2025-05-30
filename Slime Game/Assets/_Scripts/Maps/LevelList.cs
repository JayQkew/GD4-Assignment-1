using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelList : MonoBehaviour
{
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

        ArrangeList();
    }

    public void ArrangeList()
    {
        list.Clear(); // Clear the list before populating it
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        if (sceneCount <= 1) return; // if there is only one scene, then there is nothing to add.

        int currentScene = SceneManager.GetActiveScene().buildIndex;

        for (int i = 0; i < roundLimit; i++)
        {
            int newScene;
            int safetyCounter = 0;
            int maxSafety = sceneCount * 2;

            do
            {
                newScene = UnityEngine.Random.Range(1, sceneCount);
                safetyCounter++;
            } while ((newScene == currentScene || list.Contains(newScene)) && safetyCounter < maxSafety);

            if (safetyCounter >= maxSafety)
            {
                Debug.LogWarning("LevelList: Could not generate unique scene index.");
                break; // Exit the loop to prevent infinite loop
            }

            list.Add(newScene);
        }
    }
    
    

    public int roundLimit;

    public static LevelList instance { get; private set; }
    public  List<int> list = new List<int>(3);
    public int listIndex = 0;

    public int SelectLevel()
    {
        return list[listIndex];
    }

    [SerializeField] public int team1Total;
    [SerializeField] public int team2Total;

    public void UpdateTotalScores(Teams scoredAgainst)
    {
        if (scoredAgainst == Teams.TeamOne)
        {
            team2Total++;
        }
        else
        {
            team1Total++;
        }
    }

    public int WhoWon()
    {
        if (team1Total > team2Total)
        {
            return 0;
        }
        else if (team1Total < team2Total)
        {
            return 1;
        }

        return 2;
    }
}