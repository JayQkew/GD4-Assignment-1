using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapCard : MonoBehaviour
{
    public Map map;
    private Button button;
    private Image image;
    
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color unselectedColor;

    private void Awake() {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
    }

    public void SetMapCard(Map newMap) {
        map = newMap;
        GetComponentInChildren<TextMeshProUGUI>().text = newMap.mapName.Substring(4, newMap.mapName.Length - 4);
        button.onClick.AddListener(ClickCard);
    }

    private void ClickCard() => SelectCard(!map.selected);

    public void SelectCard(bool selected) {
        map.selected = selected;
        image.color = selected ? selectedColor : unselectedColor;
    }
}
