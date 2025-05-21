using System;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    private PlayerStats playerStats;
    public List<Card> cards;

    private void Awake() {
        playerStats = GetComponent<PlayerStats>();
    }

    /// <summary>
    /// when the player selects a card
    /// </summary>
    /// <param name="card">the card added</param>
    public void AddCard(Card card) {
        foreach (Modifier modifier in card.modifiers) {
            playerStats.ModifyStat(modifier);
        }
        cards.Add(card);
    }
}
