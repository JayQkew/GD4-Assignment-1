using UnityEngine;

[System.Serializable]
public class Map
{
    public string mapName;
    public bool selected;

    public Map(string _mapName) {
        mapName = _mapName;
    }
}
