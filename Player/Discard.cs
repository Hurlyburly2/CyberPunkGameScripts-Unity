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

    private List<Card> FindCardsWithKeyword(string keyword)
    {
        List<Card> foundCards = new List<Card>();
        foreach (Card card in cardsInDiscard)
        {
            List<string> keywords = new List<string>(card.GetKeywords());
            if (keywords.Contains(keyword))
            {
                foundCards.Add(card);
            }
        }

        return foundCards;
    }

    public Card DrawRandomCardFromDiscard(string keyword)
    {
        List<Card> foundCards = FindCardsWithKeyword(keyword);
        if (foundCards.Count > 0)
        {
            Card cardToDraw = foundCards[Mathf.FloorToInt(Random.Range(0, foundCards.Count))];
            RemoveOneCardFromDeck(cardToDraw);
            //SetCardsInDeckTextField();
            return cardToDraw;
        }
        else
        {
            AllCards allCards = FindObjectOfType<AllCards>();
            return allCards.GetCardById(0);
        }
    }

    private void RemoveOneCardFromDeck(Card card)
    {
        int cardIndex = cardsInDiscard.IndexOf(card);
        cardsInDiscard.RemoveAt(cardIndex);
    }
}
