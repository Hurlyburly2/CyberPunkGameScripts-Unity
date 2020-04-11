using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Discard : MonoBehaviour
{
    List<Card> cardsInDiscard = new List<Card>();

    // Config
    Deck deck;

    private void Start()
    {
        deck = FindObjectOfType<Deck>();
    }

    public void AddCardToDiscard(Card card)
    {
        cardsInDiscard.Add(card);
    }

    public void ShuffleDiscardIntoDeck()
    {
        deck.AddToDeck(cardsInDiscard);
        deck.ShuffleDeck();
        cardsInDiscard.Clear();
    }

    public int GetCardCount()
    {
        return cardsInDiscard.Count;
    }
}
