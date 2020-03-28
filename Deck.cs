﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Deck : MonoBehaviour
{
    List<Card> cards = new List<Card>();
    AllCards allCards;
    GameObject cardsInDeckText;

    public void SetupDeck(List<int> cardIds)
    {
        allCards = FindObjectOfType<AllCards>();

        foreach(int cardId in cardIds)
        {
            cards.Add(allCards.GetCardById(cardId));
        }

        ShuffleDeck();

        cardsInDeckText = FindObjectOfType<ConfigData>().GetCardsInDeckTextField();
        SetCardsInDeckTextField();
    }

    private void SetCardsInDeckTextField()
    {
       cardsInDeckText.GetComponent<TextMeshProUGUI>().text = cards.Count.ToString();
    }

    public void ShuffleDeck()
    {
        System.Random _random = new System.Random();

        Card myGO;

        int n = cards.Count;
        for (int i = 0; i < n; i++)
        {
            // NextDouble returns a random number between 0 and 1.
            // ... It is equivalent to Math.random() in Java.
            int r = i + (int)(_random.NextDouble() * (n - i));
            myGO = cards[r];
            cards[r] = cards[i];
            cards[i] = myGO;
        }
    }

    public void DebugLogDeck()
    {
        Debug.Log("Deck size: " + cards.Count);

        string cardOrder = "";

        foreach (Card card in cards)
        {
            cardOrder += card.GetCardId() + " ";
        }

        Debug.Log("Deck Order Randomized: " + cardOrder);
    }
}
