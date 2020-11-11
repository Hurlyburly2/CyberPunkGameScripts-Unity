using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeck : MonoBehaviour
{
    [SerializeField] List<EnemyCard> cards = new List<EnemyCard>();
    EnemyDiscard enemyDiscard;

    bool shouldShuffle;

    public void SetupDeck(bool newShouldShuffle)
    {
        enemyDiscard = FindObjectOfType<EnemyDiscard>();
        shouldShuffle = newShouldShuffle;

        if (shouldShuffle)
        {
            ShuffleDeck();
        }
    }

    public EnemyCard DrawTopCard()
    {
        if (cards.Count <= 0)
        {
            enemyDiscard.ShuffleDiscardIntoDeck();
        }
        EnemyCard cardToDraw = cards[0];
        cards.RemoveAt(0);
        return cardToDraw;
    }

    public void ShuffleGeneratedCardsIntoDeck(List<int> cardsToAddIds)
    {
        AllEnemyCards allEnemyCards = FindObjectOfType<AllEnemyCards>();
        List<EnemyCard> cardsToAdd = new List<EnemyCard>();

        foreach(int cardId in cardsToAddIds)
        {
            cardsToAdd.Add(allEnemyCards.GetEnemyCardById(cardId));
        }
        AddCardToDeck(cardsToAdd);
        ShuffleDeck();
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

    public int GetCardsInDeckCount()
    {
        return cards.Count;
    }

    public int RemoveAllVirusCards()
    {
        int trapCardCount = 0;
        bool trapsExist = true;
        while (trapsExist)
        {
            int cardToRemove = -1;
            for(int i = 0; i < cards.Count; i++)
            {
                if (cards[i].IsVirus())
                {
                    cardToRemove = i;
                    trapCardCount++;
                    break;
                }
            }
            if (cardToRemove != -1)
            {
                cards.RemoveAt(cardToRemove);
            } else
            {
                trapsExist = false;
            }
        }
        return trapCardCount;
    }
}
