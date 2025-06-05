using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapCard : MonoBehaviour
{
    public Map map;
    private Button button;

    private void Awake() {
        button = GetComponent<Button>();
    }

    public void SetMapCard(Map newMap) {
        map = newMap;
        GetComponentInChildren<TextMeshProUGUI>().text = newMap.mapName.Substring(4, newMap.mapName.Length - 4);
        button.onClick.AddListener(ClickCard);
    }

    private void ClickCard() => map.selected = !map.selected;
    
}
