using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardLogic : MonoBehaviour,
    ISelectHandler,
    IDeselectHandler,
    ISubmitHandler,
    ICancelHandler
{
    public Card card;
    [SerializeField] private TextMeshProUGUI header;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Image image;

    public void SetCard(Card c) {
        card = c;
        //get ui and make it match
        header.text = c.cardName;
        description.text = c.description;
        image.sprite = c.icon;
    }

    public void OnSelect(BaseEventData eventData) {
    }

    public void OnDeselect(BaseEventData eventData) {
    }

    public void OnSubmit(BaseEventData eventData) {
        Debug.Log(card.cardName + " submitted");
        //add card to players deck (need to get the active player)
        Deck deck = GameManager.Instance.draftState.lostPlayerDeck;
        deck.AddCard(card);
        GameManager.Instance.draftState.cards.Remove(card);
        GameManager.Instance.SwitchState(GameState.Round);
    }

    public void OnCancel(BaseEventData eventData) {
    }
}
