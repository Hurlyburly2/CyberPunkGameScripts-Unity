using System.Collections;
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

    public Card DrawCardFromTop()
    {
        Card cardToDraw = cards[0];
        cards.RemoveAt(0);
        SetCardsInDeckTextField();

        return cardToDraw;
    }

    public void AddToDeck(Card newCard)
    {
        cards.Add(newCard);
        SetCardsInDeckTextField();
    }

    public void AddToDeck(List<Card> newCards)
    {
        cards.AddRange(newCards);
        SetCardsInDeckTextField();
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

    public int GetCardCount()
    {
        return cards.Count;
    }

    public Card DrawRandomCardFromDeck(string keyword)
    {
        List<Card> foundCards = FindCardsWithKeyword(keyword);
        if (foundCards.Count > 0)
        {
            Card cardToDraw = foundCards[Mathf.FloorToInt(Random.Range(0, foundCards.Count))];
            RemoveOneCardFromDeck(cardToDraw);
            SetCardsInDeckTextField();
            return cardToDraw;
        } else
        {
            return allCards.GetCardById(0);
        }
    }

    private void RemoveOneCardFromDeck(Card card)
    {
        int cardIndex = cards.IndexOf(card);
        cards.RemoveAt(cardIndex);
    }

    private List<Card> FindCardsWithKeyword(string keyword)
    {
        List<Card> foundCards = new List<Card>();
        foreach(Card card in cards)
        {
            List<string> keywords = new List<string>(card.GetKeywords());
            if (keywords.Contains(keyword))
            {
                foundCards.Add(card);
            }
        }

        return foundCards;
    }
}
