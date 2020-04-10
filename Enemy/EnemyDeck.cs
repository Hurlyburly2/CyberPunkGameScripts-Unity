﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeck : MonoBehaviour
{
    [SerializeField] List<EnemyCard> cards = new List<EnemyCard>();
    EnemyDiscard enemyDiscard;

    public void SetupDeck()
    {
        enemyDiscard = FindObjectOfType<EnemyDiscard>();
        ShuffleDeck();
    }

    public EnemyCard DrawTopCard()
    {
        DebugLogDeck();
        if (cards.Count <= 0)
        {
            enemyDiscard.ShuffleDiscardIntoDeck();
        }
        EnemyCard cardToDraw = cards[0];
        cards.RemoveAt(0);
        return cardToDraw;
    }

    public void AddCardToDeck(List<EnemyCard> cardsToAdd)
    {
        cards.AddRange(cardsToAdd);
    }

    public void ShuffleDeck()
    {
        System.Random _random = new System.Random();

        EnemyCard myGO;

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

        foreach (EnemyCard card in cards)
        {
            cardOrder += card.GetCardId() + " ";
        }

        Debug.Log("Card Order: " + cardOrder);
    }
}
