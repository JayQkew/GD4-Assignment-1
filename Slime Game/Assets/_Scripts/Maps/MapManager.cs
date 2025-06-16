using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    public Map[] maps;
    public List<string> mapPool = new List<string>();
    public List<string> previousMaps = new List<string>();
    
    public List<int> list = new List<int>(3);
    public int roundLimit;
    public int listIndex = 0;
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
        // Get all scenes in the build
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        // Initialise a list that will hold the names of the maps
        List<string> mapNames = new List<string>();

        //Iterate through each scene
        for (int i = 0; i < sceneCount; i++) {
            //Get the path of the scene
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            //Get the name of the path above without the .scene extension
            string sceneName = Path.GetFileNameWithoutExtension(path);
            //Check that the scene ends in _Map
            if (sceneName.Split('_')[0] == "Map") {
                //If so, add it to the mapNames list initialised above.
                mapNames.Add(sceneName);
            }
        }
        maps = new Map[mapNames.Count];

        for (int i = 0; i < mapNames.Count; i++) {
            maps[i] = new Map(mapNames[i]);
            mapPool.Add(mapNames[i]);
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

    public void GetSelectedMaps() {
        mapPool.Clear();
        previousMaps.Clear();
        foreach (Map map in maps) {
            if (map.selected) mapPool.Add(map.mapName);
        }
        
        if (mapPool.Count == 0) {
            foreach (Map map in maps) {
                mapPool.Add(map.mapName);
            }
        }
    }
}