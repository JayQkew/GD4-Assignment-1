using TMPro;
using UnityEngine;

public class MapCard : MonoBehaviour
{
    public Map map;
    
    public void SetMapCard(Map newMap) {
        map = newMap;
        GetComponentInChildren<TextMeshProUGUI>().text = newMap.mapName.Substring(4, newMap.mapName.Length - 4);
    }
}
