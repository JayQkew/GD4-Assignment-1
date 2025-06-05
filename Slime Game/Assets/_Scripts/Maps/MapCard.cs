using UnityEngine;

[System.Serializable]
public class MapCard
{
    public string mapName;
    public bool selected;

    public MapCard(string _mapName) {
        mapName = _mapName;
    }
}
