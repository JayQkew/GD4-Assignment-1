using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class MapSelectState : GameBaseState
{
    public List<Map> selecedMaps = new List<Map>();
    public Map[] allMaps;
    [SerializeField] private GameObject mapCardPrefab;
    [SerializeField] private Transform mapCardsParent;
    public override void EnterState(GameManager manager) {
        allMaps = MapManager.Instance.maps;
        foreach (Map map in allMaps) {
            GameObject mapCard = GameObject.Instantiate(mapCardPrefab, mapCardsParent);
            mapCard.GetComponent<MapCard>().SetMapCard(map);
        }
    }

    public override void UpdateState(GameManager manager) {
        //if the player's have selected a map
    }

    public override void ExitState(GameManager manager) {
        //clear all the cards
    }
}
