using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeck : MonoBehaviour
{
    [SerializeField] List<Card> cards = new List<Card>();

    public void SetupDeck()
    {
        ShuffleDeck();
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

        Debug.Log("Card Order: " + cardOrder);
    }
}
