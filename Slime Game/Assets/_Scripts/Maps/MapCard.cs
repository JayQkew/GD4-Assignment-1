using UnityEngine;

[System.Serializable]
public class MapCard
{
    public string MapName { get; private set; }
    public bool Selected { get; private set; }

    public MapCard(string mapName) {
        MapName = mapName;
    }

    public void SetMapName(string mapName) {
        MapName = mapName;
    }

    public void SetSelected(bool selected) {
        Selected = selected;
    }
}
