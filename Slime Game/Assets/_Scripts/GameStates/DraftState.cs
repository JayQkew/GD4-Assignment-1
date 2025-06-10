using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

[Serializable]
public class DraftState : GameBaseState
{
    [SerializeField] private AllCards allCards;
    [SerializeField] private int draftSize;
    [SerializeField] private GameObject cardPrefab;
    private Transform draftParent;
    
    public override void EnterState(GameManager manager) {
        SceneManager.LoadScene("Draft");
        SceneManager.sceneLoaded += OnSceneLoaded;
        //disable the other players ui selection
    }

    public override void UpdateState(GameManager manager) {
    }

    public override void ExitState(GameManager manager) {
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        //draft cards
        draftParent = GameObject.Find("Draft").transform;
        Card[] draft = DraftedCards();
        foreach (Card card in draft) {
            GameObject cardObject = Object.Instantiate(cardPrefab, draftParent);
            cardObject.GetComponent<CardLogic>().SetCard(card);
        }
        //show current players cards
    }

    private Card[] DraftedCards() {
        List<Card> _allCards = allCards.cards.ToList();
        Card[] cards = new Card[draftSize];
        for (int i = 0; i < draftSize; i++) {
            Card card = _allCards[Random.Range(0, _allCards.Count)];
            cards[i] = card;
            _allCards.Remove(card);
        }
        
        return cards;
    }
}
