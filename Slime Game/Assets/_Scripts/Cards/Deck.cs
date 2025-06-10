using System;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    private PlayerStats playerStats;
    public List<Card> cards;
    public Transform abilitiesParent;

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

        foreach (GameObject ability in card.abilities) {
            Instantiate(ability, Vector3.zero, Quaternion.identity, abilitiesParent);
        }
        cards.Add(card);
    }
}
