using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    public MapCard[] mapCards;
    public List<string> mapPool = new List<string>();
    public List<string> previousMaps = new List<string>();
    
    public List<int> list = new List<int>(3);
    public int roundLimit;
    public int listIndex = 0;

    [SerializeField] public int team1Total;
    [SerializeField] public int team2Total;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }

        // ArrangeList();
        GetAllMaps();
    }

    private void GetAllMaps() {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        List<string> maps = new List<string>();

        for (int i = 0; i < sceneCount; i++) {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = Path.GetFileNameWithoutExtension(path);
            if (sceneName.Substring(0, 3) == "Map") {
                maps.Add(sceneName);
            }
        }
        mapPool = maps; //temporary
        mapCards = new MapCard[maps.Count];

        foreach (string map in maps) {
            mapCards[0] = new MapCard(map);
            mapPool.Add(map);
        }
    }

    public void NextMap() {
        int randIndex = Random.Range(0, mapPool.Count);
        string map = mapPool[randIndex];
        mapPool.RemoveAt(randIndex);
        previousMaps.Add(map);

        if (mapPool.Count == 0) {
            mapPool = previousMaps.ToList();
            previousMaps.Clear();
        }
        
        SceneManager.LoadScene(map);
    }

    public void ArrangeList() {
        list.Clear(); // Clear the list before populating it
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        if (sceneCount <= 1) return; // if there is only one scene, then there is nothing to add.

        int currentScene = SceneManager.GetActiveScene().buildIndex;

        for (int i = 0; i < roundLimit; i++) {
            int newScene;
            int safetyCounter = 0;
            int maxSafety = sceneCount * 2;

            do {
                newScene = UnityEngine.Random.Range(1, sceneCount);
                safetyCounter++;
            } while ((newScene == currentScene || list.Contains(newScene)) && safetyCounter < maxSafety);

            if (safetyCounter >= maxSafety) {
                Debug.LogWarning("LevelList: Could not generate unique scene index.");
                break; // Exit the loop to prevent infinite loop
            }

            list.Add(newScene);
        }
    }

    public int SelectLevel() {
        return list[listIndex];
    }

    public void UpdateTotalScores(Teams scoredAgainst) {
        if (scoredAgainst == Teams.TeamOne) {
            team2Total++;
        }
        else {
            team1Total++;
        }
    }
    public int WhoWon() {
        if (team1Total > team2Total) {
            return 0;
        }
        else if (team1Total < team2Total) {
            return 1;
        }

        return 2;
    }
}