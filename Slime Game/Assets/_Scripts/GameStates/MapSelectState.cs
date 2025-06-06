using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

[Serializable]
public class MapSelectState : GameBaseState
{
    [SerializeField] private GameObject mapCardPrefab;
    [SerializeField] private Transform mapCardsParent;

    private bool selectAll = false;
    private List<MapCard> mapCards = new List<MapCard>();
    public override void EnterState(GameManager manager) {
        // SceneManager.LoadScene("SelectMap");
        GameObject.Find("SelectAll").GetComponent<Button>().onClick.AddListener(SelectAllMaps);
        GameObject.Find("Done Button").GetComponent<Button>().onClick.AddListener(() => {
            manager.SwitchState(GameState.Lobby);
        });
        
        foreach (Map map in MapManager.Instance.maps) {
            GameObject mapCard = GameObject.Instantiate(mapCardPrefab, mapCardsParent);
            mapCard.GetComponent<MapCard>().SetMapCard(map);
            mapCards.Add(mapCard.GetComponent<MapCard>());
        }
    }

    public override void UpdateState(GameManager manager) {
        //if the player's have selected a map
    }

    public override void ExitState(GameManager manager) {
        MapManager.Instance.GetSelectedMaps();
    }

    public void SelectAllMaps() {
        selectAll = !selectAll;
        foreach (MapCard card in mapCards) {
            card.SelectCard(selectAll);
        }
    }
}
