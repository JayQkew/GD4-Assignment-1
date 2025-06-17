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
    private bool _firstTime = true;
    public int winner;
    public Deck lostPlayerDeck;
    [SerializeField] private AllCards allCards;
    public List<Card> cards;
    [SerializeField] private int draftSize;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform draftParent;
    
    public override void EnterState(GameManager manager) {
        SceneManager.LoadScene("Draft");
        SceneManager.sceneLoaded += OnSceneLoaded;
        //disable the other players ui selection
        //get
        if (_firstTime) {
            cards = new List<Card>();
            cards.AddRange(allCards.cards);
            _firstTime = false;
        }
    }

    public override void UpdateState(GameManager manager) {
    }

    public override void ExitState(GameManager manager) {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Multiplayer.Instance.SetUIInteraction(0, true);
        Multiplayer.Instance.SetUIInteraction(1, true);
        for (int i = 0; i < draftSize; i++) {
            Object.Destroy(draftParent.GetChild(i).gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        Multiplayer.Instance.SetUIInteraction(winner, false);
        int loser = (winner + 1) % 2;
        Multiplayer.Instance.SetUIInteraction(loser, true);

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
        Card[] selectedCards = new Card[draftSize];
        for (int i = 0; i < draftSize; i++) {
            Card card = cards[Random.Range(0, allCards.cards.Length)];
            selectedCards[i] = card;
        }
        return selectedCards;
    }
}
