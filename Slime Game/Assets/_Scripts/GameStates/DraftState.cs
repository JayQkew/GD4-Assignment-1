using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

[Serializable]
public class DraftState : GameBaseState
{
    public Deck lostPlayerDeck;
    [SerializeField] private AllCards allCards;
    [SerializeField] private int draftSize;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform draftParent;
    
    public override void EnterState(GameManager manager) {
        SceneManager.LoadScene("Draft");
        SceneManager.sceneLoaded += OnSceneLoaded;
        //disable the other players ui selection
        //get
    }

    public override void UpdateState(GameManager manager) {
    }

    public override void ExitState(GameManager manager) {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        for (int i = 0; i < draftSize; i++) {
            Object.Destroy(draftParent.GetChild(i).gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        //draft cards
        draftParent = GameObject.Find("Draft").transform;
        Card[] draft = DraftedCards();

        for (int i = 0; i < draft.Length; i++) {
            GameObject cardObject = Object.Instantiate(cardPrefab, draftParent);
            cardObject.GetComponent<CardLogic>().SetCard(draft[i]);
            if (i == 0) EventSystem.current.firstSelectedGameObject = cardObject;
        }
        //show current players cards
    }

    private Card[] DraftedCards() {
        Card[] cards = new Card[draftSize];
        for (int i = 0; i < draftSize; i++) {
            Card card = allCards.cards[Random.Range(0, allCards.cards.Length)];
            cards[i] = card;
        }
        return cards;
    }
}
